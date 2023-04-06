namespace SellerProduct.Models
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int AvailableQuantity { get; set; }
        public int Status { get; set; }
        public string Supplier { get; set; }
        // public IFromFile Description { get; set; }
    }
}
