using CoreTestFramework.Core.CrossCuttingConcern.Validation.FluentValidation;
using FluentValidation;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace CoreTestFramework.Core.Aspect.PostSharp
{
    /*
     Bir türe uygulandığında PostSharp'ın PortableFormatter tarafından kullanılmak üzere bir serileştirici oluşturmasına neden olur. 
     Aspect sınıflarında zorunludur.
    */
    [PSerializable]
    public class FluentValidationAspect : OnMethodBoundaryAspect // PostSharp'ın OnMethodBoundaryAspect sınıfından türettik. 
    // Bu sayede "methodun öncesine ve sonrasına" kod enjekte etme yeteneği kazandık.
    {
        private Type _validatiorType;
        public FluentValidationAspect(Type validatorType) //Aspect kullanılırken parametre olarak hangi validator kullanılacaksa (örn: [FluentValidationAspect(typeof(ProductValidator))]) constructor üzerinden veriliyor.
        {
            _validatiorType = validatorType;
        }

        //// Methoda giriş anında devreye giriyor. 
        // Yani iş metodu daha başlamadan önce validasyon çalışıyor.
        public override void OnEntry(MethodExecutionArgs args)
        {
            // Gelen validator tipinden (örn: ProductValidator) 
            // reflection kullanarak bir instance oluşturuyoruz.
            // IValidator arayüzünden cast ediyoruz.
            var validator = (IValidator)Activator.CreateInstance(_validatiorType); 

            // Validator’ın base tipinden generic argümanı (entity tipini) alıyoruz.
            // Örn: ProductValidator : AbstractValidator<Product>
            // Buradan Product tipini çıkarmış oluyoruz.
            var entityType = _validatiorType.BaseType.GetGenericArguments()[0];

            // Methoda gönderilen parametrelerden sadece 
            // bizim validasyon yapmak istediğimiz entity tipinde olanları filtreliyoruz.
            var entities = args.Arguments.Where(t => t.GetType() == entityType);

            foreach (var entity in entities)
            {
                // Her bir entity üzerinde FluentValidation kurallarını çalıştırıyoruz.
                // Eğer validasyon başarısız olursa exception fırlatılacak.
                ValidatorTool.FluentValidate(validator, entity);
            }
        }
    }
}