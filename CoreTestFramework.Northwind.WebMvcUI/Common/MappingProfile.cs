using AutoMapper;
using CoreTestFramework.Northwind.Entities.Concrate;
using CoreTestFramework.Northwind.Entities.DTO;

namespace CoreTestFramework.Northwind.WebMvcUI.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>().ForMember(dto => dto.category, entity => entity.MapFrom(p => p.categoryName())).ForMember(dto => dto.category, entitiy => entitiy.MapFrom(p => p.supplierName()));
        }
    }
}