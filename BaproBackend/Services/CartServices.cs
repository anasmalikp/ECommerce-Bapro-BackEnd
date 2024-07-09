using BaproBackend.Data;
using BaproBackend.Data.DTO;
using BaproBackend.Data.Encryption;
using BaproBackend.Data.Interfaces;
using BaproBackend.Data.Models;
using BaproBackend.Services.Interfaces;
using System.Runtime.CompilerServices;

namespace BaproBackend.Services
{
    public class CartServices : ICartServices
    {
        private readonly IDataProvider provider;
        private readonly ILogger<CartServices> logger;
        private readonly string hostUrl;
        public CartServices(IDataProvider provider, ILogger<CartServices> logger, IConfiguration config)
        {
            this.provider = provider;
            this.logger = logger;
            this.hostUrl = config["HostUrl:url"];
        }

        public async Task<bool> AddToCart(string productId, string token)
        {
            try
            {
                var product = await provider.GetByID<products>(Constants.Tables.products.ToString(), productId);
                var decoded = PasswordHasher.TokenDecode(token);
                var user = await provider.GetAllByCondition<cart>(Constants.Tables.cart.ToString(), new cart { user_id = decoded.user_id });
                logger.LogInformation($"user cart number = {user.Count()}");
                if (user.Count() == 0)
                {
                    cart cartDetails = new cart
                    {
                        id = Constants.GenerateId(),
                        user_id = decoded.user_id
                    };
                    var addToCart = await provider.Insert<cart>(Constants.Tables.cart.ToString(), cartDetails);
                    if (addToCart < 1)
                    {
                        logger.LogError("something went wrong while adding the user to cart");
                        return false;

                    }
                    cartitem productInCart = new cartitem
                    {
                        id = Constants.GenerateId(),
                        cart_id = cartDetails.id,
                        cart_qty = 1,
                        product_id = product.id,
                        total_price = product.price
                    };

                    var addToCartItem = await provider.Insert<cartitem>(Constants.Tables.cartitem.ToString(), productInCart);
                    if (addToCartItem < 1)
                    {
                        logger.LogError("something went wrong while adding the product to cart");
                        return false;
                    }
                    logger.LogInformation("product added successfully");
                    return true;
                }
                else
                {
                    var existingproduct = await provider.GetAllByCondition<cartitem>(Constants.Tables.cartitem.ToString(), new cartitem { cart_id = user.FirstOrDefault().id, product_id = product.id });
                    if (existingproduct.Count() > 0)
                    {
                        logger.LogError("product already in cart");
                        return false;
                    }
                    logger.LogInformation("after checking the product");
                    cartitem prod = new cartitem
                    {
                        id = Constants.GenerateId(),
                        cart_id = user.FirstOrDefault().id,
                        cart_qty = 1,
                        product_id = product.id,
                        total_price = product.price
                    };

                    var final = await provider.Insert<cartitem>(Constants.Tables.cartitem.ToString(), prod);
                    if (final < 1)
                    {
                        logger.LogError("something went wrong while adding");
                        return false;
                    }
                    logger.LogInformation("added");
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"{ex.Message}");
                return false;
            }
        }

        public async Task<bool> ChangeQty(string cartId, int qty)
        {
            try
            {
                var product = await provider.GetByID<cartitem>(Constants.Tables.cartitem.ToString(), cartId);
                product.cart_qty = qty;
                var result = await provider.Update<cartitem>(Constants.Tables.cartitem.ToString(), product);
                if (result < 1)
                {
                    logger.LogError($"error while updating the cart");
                    return false;
                }
                logger.LogInformation("updated successfully");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteCartItem(string cartId)
        {
            try
            {
                var result = await provider.Delete<cartitem>(Constants.Tables.cartitem.ToString(), cartId);
                if (result < 1)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<ProductsDTO>> GetAllCartItems(string token)
        {
            var user = PasswordHasher.TokenDecode(token);
            var cartdetails = await provider.GetAllByCondition<cart>(Constants.Tables.cart.ToString(), new cart { user_id = user.user_id });
            var cartproducts = await provider.GetAllByCondition<cartitem>(Constants.Tables.cartitem.ToString(), new cartitem { cart_id = cartdetails.FirstOrDefault().id });
            if (cartproducts == null)
            {
                return null;
            }
            List<ProductsDTO> cartItems = new List<ProductsDTO>();
            foreach(var prd in cartproducts)
            {
                var product = await provider.GetByID<products>(Constants.Tables.products.ToString(), prd.product_id);
                ProductsDTO productsDTO = new ProductsDTO();
                if (product != null)
                {
                    productsDTO.id = product.id;
                    productsDTO.price = prd.total_price;
                    productsDTO.image_url = hostUrl + product.image_url;
                    productsDTO.quantity = prd.cart_qty;
                    productsDTO.mrp = product.mrp * prd.cart_qty;
                    productsDTO.cart_id = prd.id;
                    cartItems.Add(productsDTO);
                }
            }
            return cartItems;
        }
    }
}
