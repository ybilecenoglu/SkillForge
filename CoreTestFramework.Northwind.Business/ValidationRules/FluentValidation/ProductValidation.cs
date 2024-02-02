using CoreTestFramework.Northwind.Entities.Model;
using FluentValidation;

namespace CoreTestFramework.Northwind.Business.ValidationRules.FluentValidation{
    public class ProductValidation : AbstractValidator<Product> {
        public ProductValidation()
        {
            RuleFor(p => p.product_name).NotNull().WithMessage("Ürün adı boş geçilemez!").MaximumLength(50).WithMessage("Ürün adı en fazla 50 karakter olmalıdır");
            RuleFor(p => p.unit_price).GreaterThan(0).WithMessage("Lütfen ürün fiyatı giriniz!.");
            RuleFor(p => p.units_in_stock).GreaterThan(0).WithMessage("Lütfen ürün stoku giriniz!.");
            
        }
    }
}