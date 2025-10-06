using Microsoft.EntityFrameworkCore;

namespace SkillForge.Entities.DTO
{
    [Keyless]
    public class CategoryDTO
    {
        public int category_id {get; set;}
        public string category_name {get; set;}
        public string description {get; set;}
        public string picture {get; set;}
    }
}