using System.Data;
using CoreTestFramework.Northwind.Entities.Model;

namespace CoreTestFramework.Northwind.Entities.DTO
{
    public class ProductDTO
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int SupplierID {get; set;}
        public int CategoryID {get; set;}
        public string CompanyName { get; set; }
        public string CategoryName { get; set; }
        public string QuantityPerUnit { get; set; }
        public double UnitPrice { get; set; }
        public int UnitsInStock {get; set;}
        public int UnitsOnOrder {get; set;}
        public int ReorderLevel {get; set;}
        public string Discontinued {get; set;}
        public bool AktifMi{get; set;}
        
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}