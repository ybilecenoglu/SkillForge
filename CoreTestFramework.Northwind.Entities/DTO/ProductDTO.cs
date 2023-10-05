namespace CoreTestFramework.Northwind.Entities.DTO
{
    public class ProductDTO
    {
        public int product_id { get; set; }

        public string product_name { get; set; } = null!;

        public string supplier { get; set; }

        public string category { get; set; }

        public string quantity_per_unit { get; set; }

        public double? unit_price { get; set; }

        public short? units_in_stock { get; set; }

        public short? units_on_order { get; set; }

        public short? reorder_level { get; set; }

        public int discontinued { get; set; }
    }
}