using BaproBackend.Data.Models;
using BaproBackend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaproBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartServices services;
        public CartController(ICartServices services)
        {
            this.services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCartProducts(string token)
        {
            var result = await services.GetAllCartItems(token);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPut("update_cart")]
        public async Task<IActionResult> UpdateQty(string cart_id, int qty)
        {
            var result = await services.ChangeQty(cart_id, qty);
            if(result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(string token, string products)
        {
            var result = await services.AddToCart(products, token);
            if(!result)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct (string Id)
        {
            var result = await services.DeleteCartItem(Id);
            if(result)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
