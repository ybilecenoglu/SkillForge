using CoreTestFramework.Core.CrossCuttingConcern.Validation.FluentValidation;
using FluentValidation;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace CoreTestFramework.Core.Aspect.PostSharp
{
    [PSerializable]//Bir türe uygulandığında PostSharp'ın PortableFormatter tarafından kullanılmak üzere bir serileştirici oluşturmasına neden olan özel öznitelik.
    //PostSharp kütüphanesinden gelen OnMethodBoundaryAspect İmplementasyonu yapıldı.
    public class FluentValidationAspect : OnMethodBoundaryAspect //PostSharp OnMethodBoundryAspect'i sınıfımıza miras vererek sınıfımızın runtime öncesinde yada sonrasında bir kod parçası ekleme özelliği kazandı.
    {
        private Type _validatiorType;
        public FluentValidationAspect(Type validatorType) //Validasyon işlemini postsharp araclığıyla gerçekleştirirken parametre olarak validasyon hangi tipte yapılacak gönderiyorum.
        {
            _validatiorType = validatorType;
        }
        
        public override void OnEntry(MethodExecutionArgs args) // Verilen methoda girildiği anda çalışan aspect metodudur.
        {
            
            var validator = (IValidator)Activator.CreateInstance(_validatiorType); //Validasyona gelen _validatorType(ProductValidator gibi.) ile IValidator tipinde bir instance oluşturduk.
            var entityType =_validatiorType.BaseType.GetGenericArguments()[0]; //Daha sonra gelen _validatorType BaseType ına giderek GetGenericArguments()[0] ile ilgili entity tipini (Product gibi.) aldık.
            var entities = args.Arguments.Where(t => t.GetType() == entityType); //Daha sonra bu tipte oluşan neleri alıp entities içersine koyduk.

            foreach (var entity in entities)
            {
                ValidatorTool.FluentValidate(validator,entity); //Daha sonra ValidatorTool sınıfında static oluşturduğumuz FluentValidate methodunu IValidator sınıfından oluşturduğumuz instance ve entity (Product, Category vs.) çalıştırdık.
            }
        }
    }
}