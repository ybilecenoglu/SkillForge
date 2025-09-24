using CoreTestFramework.Northwind.Entities.ViewModels;
using FluentValidation;

namespace CoreTestFramework.Northwind.Business.ValidationRules.FluentValidation{
    //Product sınıfımız için validasyon kurallarını oluşturduğumuz ve AbstractValidator'ı miras alan sınıfımız
    public class ProductValidation : AbstractValidator<ProductViewModel> {
        public ProductValidation()
        {
            RuleFor(p => p.Product.product_name).NotNull().WithMessage("Ürün adı boş geçilemez!").MaximumLength(50).WithMessage("Ürün adı en fazla 50 karakter olmalıdır");
            RuleFor(p => p.Product.unit_price).GreaterThan(0).WithMessage("Lütfen ürün fiyatı giriniz!.");
            RuleFor(p => p.Product.units_in_stock).GreaterThan(0).WithMessage("Lütfen ürün stoku giriniz!.");
        }
    }
}