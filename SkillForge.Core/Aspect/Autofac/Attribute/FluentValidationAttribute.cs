namespace SkillForge.Core
{
    // Attribute, metod seviyesinde kullanılmak üzere tasarlandı
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class FluentValidationAttribute : Attribute
    {
        // Validasyon için kullanılacak validator tipi (ör. ProductValidator)
        public Type ValidatorType { get; }

        // Constructor ile validator tipi veriliyor
        public FluentValidationAttribute(Type validatorType)
        {
            ValidatorType = validatorType;
        }
    }
}