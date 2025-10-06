using SkillForge.Entities.DTO;
using SkillForge.Entities.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace SkillForge.WebMvcUI.ViewModels
{
    public class ProductViewModel
    {
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
        public ProductDTO Product { get; set; } = new ProductDTO();
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public OrderDTO Order { get; set; }
        public SelectList Suppliers { get; set; }
        public SelectList Categories { get; set; }

        public int SupplierID { get; set; }
        public int CategoryID { get; set; }
    }
}