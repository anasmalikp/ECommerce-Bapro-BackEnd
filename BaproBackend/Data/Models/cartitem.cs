namespace BaproBackend.Data.Models
{
    public class cartitem
    {
        public string? id { get; set; }
        public string? cart_id { get; set; }
        public string? product_id { get; set; }
        public int? cart_qty { get; set; }
        public int? total_price { get; set; }
    }
}
