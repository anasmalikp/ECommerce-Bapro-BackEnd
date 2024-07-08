using BaproBackend.Data.DTO;
using BaproBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaproBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices services;
        public CategoryController(ICategoryServices services)
        {
            this.services = services;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAllCategory()
        {
            var result = await services.GetAllCategories();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        
        [HttpPost]
        public async Task<IActionResult> NewCategory(CategoriesDTO category)
        {
            var result = await services.AddNewCategory(category);
            if(result)
            {
                return Created();
            }
            return BadRequest();
        }
    }
}
