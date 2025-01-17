﻿using BaproBackend.Data.DTO;
using BaproBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaproBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices services;
        public ProductController(IProductServices services)
        {
            this.services = services;
        }

        [HttpGet]
       
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await services.GetAllProducts();
            if (result == null)
            {
                return NotFound("No Products Available");  
            }
            return Ok(result);
        }

        
        [HttpPost]
        /*[Authorize]*/
        public async Task<IActionResult> AddNewProduct([FromForm] ProductsDTO product, IFormFile image )
        {
            var result = await services.AddNewProduct(product, image);
            if(result)
            {
                return Ok("product added");
            }
            return BadRequest("Something went wrong");
        }

        [HttpGet("getbyid")]
        
        public async Task<IActionResult> GetByID(string productId)
        {
            var result = await services.GetProductByID(productId);
            if(result == null)
            {
                return NotFound("product not found");
            }
            return Ok(result);
        }
    }
}
