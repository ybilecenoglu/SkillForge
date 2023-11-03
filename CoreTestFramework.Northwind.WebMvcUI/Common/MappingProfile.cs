using AutoMapper;
using CoreTestFramework.Northwind.Entities.Concrate;
using CoreTestFramework.Northwind.Entities.DTO;

namespace CoreTestFramework.Northwind.WebMvcUI.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>()
            .ForMember(dto => dto.CompanyName, entity => entity.MapFrom(p => p.Supplier.CompanyName))
            .ForMember(dto => dto.CategoryName, entity => entity.MapFrom(p => p.Category.CategoryName));
        }
    }
}