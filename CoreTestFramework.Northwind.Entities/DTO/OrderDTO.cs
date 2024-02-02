using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CoreTestFramework.Northwind.Entities.DTO
{
    public class OrderDTO 
    {
        public int order_id { get; set; }
        public string customer_name {get; set;}
        public string employee_name {get; set;}
        public string shipper_name {get; set;}
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}")]
        public DateTime order_date { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}")]
        public DateTime required_date { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}")]
        public DateTime shipped_date { get; set; }
        public decimal freight { get; set; }
        public string ship_name { get; set; }
        public string ship_address { get; set; }
        public string ship_city { get; set; }
        public string ship_region { get; set; }
        public string ship_postal_code { get; set; }
        public string ship_country { get; set; }
    }
}