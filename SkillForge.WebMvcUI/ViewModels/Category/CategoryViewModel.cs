using SkillForge.Entities.DTO;
using SkillForge.Entities.DTO.Common;

namespace SkillForge.WebMvcUI.ViewModels
{
    public class CategoryViewModel 
    {
        public List<CategoryDTO> Categories {get; set;}
        public CategoryDTO Category {get; set;}

        public UploadFileDTO[] uploadFiles {get; set;}
    }
}