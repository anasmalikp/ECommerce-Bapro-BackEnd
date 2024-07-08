using AutoMapper;
using BaproBackend.Data;
using BaproBackend.Data.DTO;
using BaproBackend.Data.Interfaces;
using BaproBackend.Data.Models;
using BaproBackend.Services.Interfaces;

namespace BaproBackend.Services
{
    public class ProductServices:IProductServices
    {
        private readonly IDataProvider provider;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly string HostUrl;
        private readonly ILogger<ProductServices> logger;
        public ProductServices(IDataProvider provider, IMapper mapper, IWebHostEnvironment webHostEnvironment, IConfiguration config, ILogger<ProductServices> logger)
        {
            this.provider = provider;
            this.mapper = mapper;
            this.webHostEnvironment = webHostEnvironment;
            this.HostUrl = config["HostUrl:url"];
            this.logger = logger;
        }

        public async Task<IEnumerable<products>> GetAllProducts()
        {
            try
            {
                
                var products = await provider.GetAll<products>(Constants.Tables.products.ToString());
                foreach(var product in products)
                {
                    product.image_url = HostUrl + product.image_url;
                }
                return products;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> AddNewProduct(ProductsDTO product, IFormFile image)
        {
            try
            {
                
                string productImage = "";
                if(image != null && image.Length > 0)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    string filepath = Path.Combine(webHostEnvironment.WebRootPath,"productImage", filename);

                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    productImage = "/productImage/" + filename;
                } 
                var prd = mapper.Map<products>(product);
                prd.id = Constants.GenerateId();
                prd.image_url = productImage;
                var result = await provider.Insert<products>(Constants.Tables.products.ToString(), prd);
                if(result < 1)
                {
                    logger.LogError("Something went wrong while adding the product");
                    return false;
                }
                Console.WriteLine("product added successfully");
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> EditProduct (string productId, ProductsDTO product, IFormFile image)
        {
            try
            {
                string productImage = "";
                if(image!= null && image.Length > 0)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    string filepath = Path.Combine(webHostEnvironment.WebRootPath, "productImage", filename);

                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    productImage = "/productImage/" + filename;
                }
                var prd = await provider.GetByID<products>(Constants.Tables.products.ToString(), productId);
                prd.product_name = product.product_name;
                prd.image_url = productImage;
                prd.price = product.price;
                prd.quantity = product.quantity;
                prd.category_id = product.category_id;
                var result = await provider.Update<products>(Constants.Tables.products.ToString(),prd);
                if(result < 1)
                {
                    Console.WriteLine("something went wrong while updating the product");
                    return false;
                }
                Console.WriteLine("product updated successfully");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteProduct(string ProductId)
        {
            try
            {
                var result = await provider.Delete<products>(Constants.Tables.products.ToString(), ProductId);
                if(result < 1)
                {
                    Console.WriteLine("Deleting Failed");
                    return false;
                }
                Console.WriteLine("Deleted Successfully");
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<products> GetProductByID(string ProductId)
        {
            try
            {
                var product = await provider.GetByID<products>(Constants.Tables.products.ToString(), ProductId);
                if (product == null)
                {
                    Console.WriteLine("something went while fetching the product");
                    return null;
                }
                product.image_url = HostUrl + product.image_url;
                return product;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
