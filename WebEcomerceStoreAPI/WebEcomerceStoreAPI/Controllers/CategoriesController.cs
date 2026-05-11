using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebEcomerceStoreAPI.Base;
using WebEcomerceStoreAPI.Common;
using WebEcomerceStoreAPI.Data;
using WebEcomerceStoreAPI.IServices;

namespace WebEcomerceStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize (Roles ="Admin,User")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }
        [HttpGet]
        public async Task<IBussinessResult> GetListCategory()
        {
            return await _categoryService.GetAllCategory();
             
        }
        [HttpGet("{id}")]
        public async Task<IActionResult>GetCategoryById(Guid id)
        {
            var categoryId = await _categoryService.GetByIdCategory(id);
            if (categoryId == null)
            {
                return NotFound();
            }
            return Ok(categoryId);
        }
        [HttpPost("addOrUpdateCategory")]
        public async Task<IBussinessResult>AddOrUpdateCategory(AddOrUpdateCategoryRequest request)
        {
          
            return await _categoryService.AddOrUpdateCategory(request); 
        }
        [HttpDelete("deleteCategory")]
        public async Task<IBussinessResult>DeleteCategory(Guid Id)
        {
            return await _categoryService.DeleteCategory(Id);
        }
        [HttpGet("Pagination")] 
        public async Task<IBussinessResult> PaginationCategory([FromQuery] Guid? cursorId,[FromQuery] int limit =5)
        {
            return await _categoryService.GetPagination(cursorId, limit);
        }
        [HttpGet("GetName")]
        public async Task<IBussinessResult>GetCategoryByName(string categoryName)
        {
            return await _categoryService.GetByNameCategory(categoryName);
        }
    }
}
