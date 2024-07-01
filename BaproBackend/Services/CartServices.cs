using BaproBackend.Data;
using BaproBackend.Data.Encryption;
using BaproBackend.Data.Interfaces;
using BaproBackend.Data.Models;
using BaproBackend.Services.Interfaces;

namespace BaproBackend.Services
{
    public class CartServices : ICartServices
    {
        private readonly IDataProvider provider;
        public CartServices(IDataProvider provider)
        {
            this.provider = provider;
        }

        public async Task<bool> AddToCart(string productId, string token)
        {
            try
            {
                var product = await provider.GetByID<products>(Constants.Tables.products.ToString(), productId);
                var decoded = PasswordHasher.TokenDecode(token);
                var user = await provider.GetAllByCondition<cart>(Constants.Tables.cart.ToString(), new cart { user_id = decoded.user_id });
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
                        return false;
                    }
                    var existing = await provider.GetAllByCondition<cartitem>(Constants.Tables.cartitem.ToString(), new cartitem { cart_id = cartDetails.id, product_id = product.id });
                    if (existing != null)
                    {
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
                        return false;
                    }
                    return true;
                }
                else
                {
                    var existingproduct = await provider.GetAllByCondition<cartitem>(Constants.Tables.cartitem.ToString(), new cartitem { cart_id = user.FirstOrDefault().id, product_id = product.id });
                    if (existingproduct.Any())
                    {
                        return false;
                    }
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
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ChangeQty(string cartId, int qty)
        {
            try
            {
                var product = await provider.GetByID<cartitem>(Constants.Tables.cartitem.ToString(), cartId);
                product.cart_qty = qty;
                product.total_price = product.total_price * qty;
                var result = await provider.Update<cartitem>(Constants.Tables.cartitem.ToString(), product);
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

        public async Task<IEnumerable<cartitem>> GetAllCartItems(string token)
        {
            var user = PasswordHasher.TokenDecode(token);
            var cartdetails = await provider.GetAllByCondition<cart>(Constants.Tables.cart.ToString(), new cart { user_id = user.user_id });
            var cartproducts = await provider.GetAllByCondition<cartitem>(Constants.Tables.cartitem.ToString(), new cartitem { cart_id = cartdetails.FirstOrDefault().id });
            if (cartproducts == null)
            {
                return null;
            }
            return cartproducts;
        }
    }
}
