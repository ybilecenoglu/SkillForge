using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text.Json.Serialization;
using CoreTestFramework.Northwind.Entities.Model;
using Microsoft.EntityFrameworkCore;

namespace CoreTestFramework.Northwind.Entities.DTO
{
    public class ProductDTO
    {
       
        public int product_id { get; set; }
        public string product_name { get; set; }
        public int supplier_id {get; set;}
        public int category_id {get; set;}
        public string company_name { get; set; }
        public string category_name { get; set; }
        public string quantity_per_unit { get; set; }
        public double unit_price { get; set; }
        public int units_in_stock {get; set;}
        public int units_on_order {get; set;}
        public int reorder_level {get; set;}
        public int discontinued {get; set;}
        public bool AktifMi{get; set;}
    }
}