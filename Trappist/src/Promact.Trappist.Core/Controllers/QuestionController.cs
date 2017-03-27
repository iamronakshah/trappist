using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.ApplicationClasses;
using Promact.Trappist.DomainModel.ApplicationClasses.Question;
using Promact.Trappist.Repository.Questions;
using System.Threading.Tasks;
namespace Promact.Trappist.Core.Controllers
{
    [Route("api")]
    public class QuestionController : Controller
    {
        private readonly IQuestionRespository _questionsRepository;
        public QuestionController(IQuestionRespository questionsRepository)
        {
            _questionsRepository = questionsRepository;
        }

        #region Question API
        /// <summary>
        /// Add single multiple answer question into model
        /// </summary>
        /// <param name="singleMultipleQuestion"></param>
        /// <returns></returns>   
        [HttpPost("singlemultiplequestion")]
        public IActionResult AddSingleMultipleAnswerQuestion([FromBody]SingleMultipleQuestion singleMultipleQuestion)
        {
            _questionsRepository.AddSingleMultipleAnswerQuestion(singleMultipleQuestion.singleMultipleAnswerQuestion, singleMultipleQuestion.singleMultipleAnswerQuestionOption);
            return Ok(singleMultipleQuestion);
        }

        [HttpPost("codesnippetquestion")]
        public IActionResult AddCodeSnippetQuestion([FromBody]CodeSnippetQuestionDto codeSnippetQuestionDto)
        {
            if (codeSnippetQuestionDto == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _questionsRepository.AddCodeSnippetQuestion(codeSnippetQuestionDto);
            return Ok(codeSnippetQuestionDto);
        }

        /// <summary>
        /// Get All The Questions
        /// </summary>
        /// <returns>Questions List</returns>
        [HttpGet("question")]
        public async Task<IActionResult> GetAllQuestions()
        {
            return Ok(await _questionsRepository.GetAllQuestionsAsync()); 
        }
        #endregion
    }
}