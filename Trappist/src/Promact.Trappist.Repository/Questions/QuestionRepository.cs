using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.DomainModel.ApplicationClasses.SingleMultipleAnswerQuestionApplicationClass;
using Promact.Trappist.DomainModel.DbContext;
using Promact.Trappist.DomainModel.Models.Question;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Promact.Trappist.Repository.Questions
{
    public class QuestionRepository : IQuestionRespository
    {
        private readonly TrappistDbContext _dbContext;
        public QuestionRepository(TrappistDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get all questions
        /// </summary>
        /// <returns>Question list</returns>
        public ICollection<SingleMultipleAnswerQuestionApplicationClass> GetAllQuestions()
        {
            var questions = _dbContext.SingleMultipleAnswerQuestion.ProjectTo<SingleMultipleAnswerQuestionApplicationClass>().ToList();
            questions.AddRange(_dbContext.CodeSnippetQuestion.ProjectTo<SingleMultipleAnswerQuestionApplicationClass>().ToList());
            var questionsOrderedByCreatedDateTime = questions.OrderBy(f => f.CreatedDateTime).ToList();
            return questionsOrderedByCreatedDateTime;
        }

        /// <summary>
        /// A method to add single multiple answer question.
        /// </summary>
        /// <param name="singleMultipleAnswerQuestion"></param>
        /// <param name="singleMultipleAnswerQuestionOption"></param>
        public async Task<QuestionAC>  AddSingleMultipleAnswerQuestionAsync(QuestionAC questionAC)
        {
            await _dbContext.SingleMultipleAnswerQuestion.AddAsync(questionAC.SingleMultipleAnswerQuestionAC.SingleMultipleAnswerQuestion);
            foreach (SingleMultipleAnswerQuestionOption singleMultipleAnswerQuestionOptionElement in questionAC.SingleMultipleAnswerQuestionAC.SingleMultipleAnswerQuestionOption)
            {
                //To-Do Change according to new model singleMultipleAnswerQuestionOptionElement.SingleMultipleAnswerQuestionID = singleMultipleAnswerQuestion.Id;
                await _dbContext.SingleMultipleAnswerQuestionOption.AddAsync(singleMultipleAnswerQuestionOptionElement);
            }
            _dbContext.SaveChanges();
            return(questionAC);
        }

        /// <summary>
        /// Add new code snippet question to the database
        /// </summary>
        /// <param name="codeSnippetQuestion">Code Snippet Question Model</param>
        public async Task AddCodeSnippetQuestion(QuestionAC questionAC)
        {
            var codeSnippetQuestionModel = questionAC.CodeSnippetQuestionAC;
            CodeSnippetQuestion codeSnippetQuestion = Mapper.Map<CodeSnippetQuestionAC, CodeSnippetQuestion>(codeSnippetQuestionModel);

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                //Add common question details
                var question = await _dbContext.Question.AddAsync(questionAC.Question);
                await _dbContext.SaveChangesAsync();

                var questionId = question.Entity.Id;

                //Add codeSnippet part of question
                codeSnippetQuestion.QuestionId = questionId;
                var codingQuestion = await _dbContext.CodeSnippetQuestion.AddAsync(codeSnippetQuestion);
                await _dbContext.SaveChangesAsync();

                var codingQuestionId = codingQuestion.Entity.Id;
                var codingLanguageList = codeSnippetQuestionModel.LanguageList; 

                //Map language to codeSnippetQuestion
                foreach (var language in codingLanguageList)
                {
                    var languageId = await _dbContext.CodingLanguage.Where(x => x.Language == language).Select(x => x.Id).FirstOrDefaultAsync();
                    await _dbContext.QuestionLanguageMapping.AddAsync(new QuestionLanguageMapping
                    {
                        QuestionId = codingQuestionId,
                        LanguageId = languageId
                    });
                }
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
            }
        }
    }
}