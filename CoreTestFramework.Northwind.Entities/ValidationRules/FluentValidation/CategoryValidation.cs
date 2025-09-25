using CoreTestFramework.Northwind.Entities.Model;
using FluentValidation;

namespace  CoreTestFramework.Northwind.Entities.ValidationRules.FluentValidation
{
    public class CategoryValidation : AbstractValidator<Category> 
    {
        public CategoryValidation()
        {
            RuleFor(c => c.category_name).NotEmpty().WithMessage("Kategori adı boş geçilemez.");
            RuleFor(c => c.description).MaximumLength(100).WithMessage("Kategori açıklamazı maximum 100 karakter olmalıdır.");
        }
    }
}