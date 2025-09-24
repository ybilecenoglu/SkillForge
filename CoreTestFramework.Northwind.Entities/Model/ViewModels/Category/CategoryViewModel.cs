using CoreTestFramework.Northwind.Entities.DTO;
using CoreTestFramework.Northwind.Entities.DTO.Common;

namespace CoreTestFramework.Northwind.Entities.ViewModels
{
    public class CategoryViewModel 
    {
        public List<CategoryDTO> Categories {get; set;}
        public CategoryDTO Category {get; set;}

        public UploadFileDTO[] uploadFiles {get; set;}
    }
}