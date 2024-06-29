using AutoMapper;
using BaproBackend.Data;
using BaproBackend.Data.DTO;
using BaproBackend.Data.Interfaces;
using BaproBackend.Data.Models;
using BaproBackend.Services.Interfaces;

namespace BaproBackend.Services
{
    public class CategoryServices:ICategoryServices
    {
        private readonly IDataProvider provider;
        private readonly IMapper mapper;
        public CategoryServices(IDataProvider provider, IMapper mapper)
        {
            this.provider = provider;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<category>> GetAllCategories()
        {
            try
            {
                var categories = await provider.GetAll<category>(Constants.Tables.category.ToString());
                if(categories==null)
                {
                    Console.WriteLine("something went wrong while fetching categories");
                    return null;
                }
                Console.WriteLine("categories fetched successfully");
                return categories;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> AddNewCategory(CategoriesDTO category)
        {
            try
            {
                var ctg = mapper.Map<category>(category);
                ctg.id = Constants.GenerateId();
                var result = await provider.Insert<category>(Constants.Tables.category.ToString(), ctg);
                if(result < 1)
                {
                    Console.WriteLine("something went wrong while adding category");
                    return false;
                }
                Console.WriteLine("category added successfully");
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateCategory( string categoryId,  CategoriesDTO categoryName)
        {
            try
            {
                var ctg = await provider.GetByID<category>(Constants.Tables.category.ToString(), categoryId);
                if(ctg == null)
                {
                    Console.WriteLine("category not found");
                    return false;
                }
                ctg.category_name = categoryName.category_name;
                var result = await provider.Update<category>(Constants.Tables.category.ToString(), ctg);
                if(result < 1)
                {
                    Console.WriteLine("something went wrong while updating category");
                    return false;
                }
                Console.WriteLine("category updated successfully");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteCategory(string categoryId)
        {
            try
            {
                var result = await provider.Delete<category>(Constants.Tables.category.ToString(), categoryId);
                if (result < 1)
                {
                    Console.WriteLine("something went wrong while deleting category");
                    return false;
                }
                Console.WriteLine("deleted successfully");
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
