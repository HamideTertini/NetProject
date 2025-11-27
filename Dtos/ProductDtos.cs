namespace ProductApi.Dtos
{
    
    public class ProductCreateDto
    {
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }

    
    public class ProductUpdateDto
    {
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }

   
    public class ProductReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public DateTime CreatedAt { get; set; }

      
        public bool InStock { get; set; }
    }
}
