using CoreTestFramework.Northwind.Entities.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreTestFramework.Northwind.WebMvcUI.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
            Products = new List<ProductDTO>();
            Product = new ProductDTO();
        }
        public List<ProductDTO> Products {get; set;}
        public ProductDTO Product {get; set;}

        public SelectList Suppliers {get; set;}
        public SelectList Categories {get; set;}

        public int SupplierID {get; set;}
        public int CategoryID {get; set;}
    }
}