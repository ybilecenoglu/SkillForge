using AutoMapper;
using SkillForge.Entities.Model;
using SkillForge.Entities.DTO;

namespace SkillForge.WebMvcUI.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>()
            .ForMember(dto => dto.company_name, entity => entity.MapFrom(p => p.Supplier.company_name))
            .ForMember(dto => dto.category_name, entity => entity.MapFrom(p => p.Category.category_name));

            CreateMap<Order, OrderDTO>();
            // .ForMember(dto => dto.customer_name, entity => entity.MapFrom(o => o.Customer.contact_name))
            // .ForMember(dto => dto.employee_name, entity => entity.MapFrom(o => o.Employee.first_name + " " + o.Employee.last_name))
            // .ForMember(dto => dto.shipper_name, entity => entity.MapFrom(o => o.Shipper.company_name));

            CreateMap<Category, CategoryDTO>();
        }
    }
}