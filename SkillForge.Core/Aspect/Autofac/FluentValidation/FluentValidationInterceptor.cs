using Castle.DynamicProxy; // Autofac DynamicProxy için gerekli namespace
using FluentValidation;     // FluentValidation için gerekli namespace

namespace SkillForge.Core.Aspect.Autofac
{
    // Generic interceptor sınıfı
    // TValidator  : Validator tipi (ör. ProductValidaton)
    // TEntity     : Validator’un çalışacağı entity tipi (ör. Product)
    public class FluentValidationInterceptor<TValidator, TEntity> : IInterceptor
        where TValidator : IValidator<TEntity> // TValidator mutlaka TEntity tipini validate edebilen IValidator olmalı
    {
        private readonly TValidator _validator;

        // Constructor
        // Autofac DI container bu constructor'ı kullanarak TValidator instance'ını inject eder
        public FluentValidationInterceptor(TValidator validator)
        {
            _validator = validator;
        }

        // Intercept metodu, service metoduna çağrı yapılmadan önce ve sonra çalışır
        public void Intercept(IInvocation invocation)
        {
            // if (!invocation.Method.Name.Equals("")) //sadece Create/Add metodunu kontrol et
            // {
            //     invocation.Proceed();
            //     return;
            // }

            //Eğer metod üzerinde FluentValidationAttribute yoksa normal şekilde devam et
            var attribute = invocation.Method.GetCustomAttributes(true)
                        .OfType<FluentValidationAttribute>()
                        .FirstOrDefault();

            // Eğer metod üzerinde ValidationAttribute yoksa
            if (attribute == null)
            {
                invocation.Proceed();
                return;
            }

            // Metodun parametrelerini kontrol et
            foreach (var arg in invocation.Arguments)
            {
                // Eğer parametre null değil ve TEntity tipindeyse validate et
                if (arg != null && arg is TEntity entity)
                {
                    // FluentValidation context oluştur
                    var context = new ValidationContext<TEntity>(entity);

                    // Validasyonu çalıştır
                    var result = _validator.Validate(context);

                    // Eğer validation başarısızsa exception fırlat
                    if (!result.IsValid)
                        throw new ValidationException(result.Errors);
                }
            }

            // Tüm validasyonlar geçerse, methodun asıl çalışmasına izin ver
            invocation.Proceed();
        }
    }
}