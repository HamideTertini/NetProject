
namespace ProductApi.Entities
{
    public class Product
    {
        public int Id { get; set; }              // primary key
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
