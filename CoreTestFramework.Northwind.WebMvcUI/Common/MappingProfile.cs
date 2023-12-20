using AutoMapper;
using CoreTestFramework.Northwind.Entities.Model;
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

            CreateMap<Order, OrderDTO>()
            .ForMember(dto => dto.CustomerName, entity => entity.MapFrom(o => o.Customer.ContactName))
            .ForMember(dto => dto.EmployeeName, entity => entity.MapFrom(o => o.Employee.FirstName + " " + o.Employee.LastName))
            .ForMember(dto => dto.ShipName, entity => entity.MapFrom(o => o.Shipper.CompanyName));

        }
    }
}