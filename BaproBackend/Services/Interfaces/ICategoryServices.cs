using BaproBackend.Data.DTO;
using BaproBackend.Data.Models;

namespace BaproBackend.Services.Interfaces
{
    public interface ICategoryServices
    {
        Task<IEnumerable<category>> GetAllCategories();
        Task<bool> AddNewCategory(CategoriesDTO category);
        Task<bool> UpdateCategory(string categoryId, CategoriesDTO categoryName);
        Task<bool> DeleteCategory(string categoryId);
    }
}
