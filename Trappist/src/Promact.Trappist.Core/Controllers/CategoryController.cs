using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.Models.Category;
using Promact.Trappist.Repository.Categories;
using System.Threading.Tasks;

namespace Promact.Trappist.Core.Controllers
{
    [Route("api/category")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>Category List</returns>
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categoryList = _categoryRepository.GetAllCategories();
            return Ok(categoryList);
        }

        #region post Method 
        [HttpPost]
        /// <summary>
        /// Post Method 
        /// Will Add a Catagory Name in Category Model
        ///</summary>
        /// <param name="category">Object of  class Category</param>
        /// <returns>object of the class </returns>
        public IActionResult CatagoryAdd([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _categoryRepository.AddCategory(category);
            return Ok(category);
        }
        #endregion

        #region PutMethod
        /// <summary>
        /// Put Method
        /// Will Edit a Existing Category from Category Table
        /// </summary>
        /// <param name="Id">Id is the primary key of Category Model</param>
        /// <param name="catagory">Object of  class Category</param>
        /// <returns>object of the class if key found or else it will return Bad request</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> CategoryEdit(int Id, [FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var previousCategory = await _categoryRepository.GetCategory(Id);
            if (previousCategory == null)
            {
                return NotFound();
            }
            previousCategory.CategoryName = category.CategoryName;
            _categoryRepository.CategoryEdit(previousCategory);
            return Ok(category);
            #endregion
        }

        /// <summary>
        /// Delete Method 
        ///this method control the delete operation for category and return a category Id
        ///</summary>
        /// <param name="categoryId">Id of category</param>
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> CategoryRemove([FromRoute] int categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var categoryData = await _categoryRepository.GetCategory(categoryId);
            if (categoryData != null)
            {
                await _categoryRepository.RemoveCategoryToDatabaseAsync(categoryData);
                return Ok(categoryId);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
