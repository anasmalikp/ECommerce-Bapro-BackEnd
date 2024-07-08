using BaproBackend.Data.DTO;
using BaproBackend.Data.Models;

namespace BaproBackend.Services.Interfaces
{
    public interface ICartServices
    {
        Task<bool> AddToCart(string productId, string token);
        Task<bool> ChangeQty(string cartId, int qty);
        Task<bool> DeleteCartItem(string cartId);
        Task<IEnumerable<ProductsDTO>> GetAllCartItems(string token);
    }
}
