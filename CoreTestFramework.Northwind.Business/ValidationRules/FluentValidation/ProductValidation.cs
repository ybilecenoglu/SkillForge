using CoreTestFramework.Northwind.Entities.Model;
using FluentValidation;

namespace CoreTestFramework.Northwind.Business.ValidationRules.FluentValidation{
    public class ProductValidation : AbstractValidator<Product> {
        public ProductValidation()
        {
            RuleFor(p => p.ProductName).NotNull().WithMessage("Ürün adı boş geçilemez!").MaximumLength(50).WithMessage("Ürün adı en fazla 50 karakter olmalıdır");
            RuleFor(p => p.UnitPrice).GreaterThan(0).WithMessage("Lütfen ürün fiyatı giriniz!.");
            RuleFor(p => p.UnitsInStock).GreaterThan(0).WithMessage("Lütfen ürün stoku giriniz!.");
            
        }
    }
}