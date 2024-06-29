using BaproBackend.Data.DTO;
using BaproBackend.Data.Models;

namespace BaproBackend.Services.Interfaces
{
    public interface IProductServices
    {
        Task<IEnumerable<products>> GetAllProducts();
        Task<bool> AddNewProduct(ProductsDTO product, IFormFile image);
        Task<bool> EditProduct(string productId, ProductsDTO product, IFormFile image);
        Task<bool> DeleteProduct(string ProductId);
    }
}
