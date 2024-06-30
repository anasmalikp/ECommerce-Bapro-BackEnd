namespace BaproBackend.Data.Models
{
    public class products
    {
        public string? id { get; set; }
        public string? product_name { get; set; }
        public int? quantity { get; set; }
        public int? price { get; set; }
        public string? category_id { get; set; }
        public string? image_url { get; set; }
        public string? description { get; set; }
        public int? mrp { get; set; }
    }
}
