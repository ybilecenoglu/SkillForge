namespace CoreTestFramework.Northwind.Entities.DTO
{
    public class ProductDTO
    {
        public int ProductID { get; set; }

        public string ProductName { get; set; } = null!;

        public string Supplier { get; set; }

        public string Category { get; set; }

        public string Unit { get; set; }

        public double? Price { get; set; }
    }
}