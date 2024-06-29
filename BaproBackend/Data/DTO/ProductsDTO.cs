namespace BaproBackend.Data.DTO
{
    public class ProductsDTO
    {
        public string product_name { get; set; }
        public int? quantity { get; set; }
        public int? price { get; set; }
        public string category_id { get; set; }
    }
}
