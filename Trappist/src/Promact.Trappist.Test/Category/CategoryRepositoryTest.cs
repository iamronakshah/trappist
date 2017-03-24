using Promact.Trappist.DomainModel.DbContext;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System;
using System.Linq;
using Promact.Trappist.Repository.Categories;
using System.Threading.Tasks;

namespace Promact.Trappist.Test.Category
{
    [Collection("Register Dependency")]
    public class CategoryRepositoryTest
    {

        private readonly Bootstrap _bootstrap;
        private readonly TrappistDbContext _trappistDbContext;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryRepositoryTest(Bootstrap bootstrap)
        {
            _bootstrap = bootstrap;
            //resolve dependency to be used in tests
            _trappistDbContext = _bootstrap.ServiceProvider.GetService<TrappistDbContext>();
            _categoryRepository = _bootstrap.ServiceProvider.GetService<ICategoryRepository>();
            ClearDatabase.ClearDatabaseAndSeed(_trappistDbContext);
        }

        [Fact]
        public void AddCategory()
        {
            var category = CreateCategory();
            _categoryRepository.AddCategory(category);
            Assert.True(_trappistDbContext.Category.Count() == 1);
        }

        [Fact]
        public async Task UpdateCategory()
        {
            var category = CreateCategory();
            _categoryRepository.AddCategory(category);
            var categoryToUpdate = await _categoryRepository.GetCategory(category.Id);
            Assert.NotNull(categoryToUpdate);
            if (categoryToUpdate != null)
                categoryToUpdate.CategoryName = "Updated Category";
            _categoryRepository.CategoryEdit(categoryToUpdate);
            Assert.True(_trappistDbContext.Category.Count(x => x.CategoryName == "Updated Category") == 1);
        }

        /// <summary>
        /// This is unit testing method. aim of this method is check a category remove from database or not
        /// </summary>
        
        [Fact]
        public async Task DeleteCategory()
        {
            var category = CreateCategory();
            _categoryRepository.AddCategory(category);
            var deleteCategory = await _categoryRepository.GetCategory(category.Id);
            if (deleteCategory != null)
            {
                await _categoryRepository.RemoveCategoryAsync(deleteCategory);
                Assert.Equal(0, _trappistDbContext.Category.Count());
            }
        }

        private DomainModel.Models.Category.Category CreateCategory()
        {
            var category = new DomainModel.Models.Category.Category
            {
                CategoryName = "test category",
                CreatedDateTime = DateTime.UtcNow
            };
            return category;
        }
    }
}
