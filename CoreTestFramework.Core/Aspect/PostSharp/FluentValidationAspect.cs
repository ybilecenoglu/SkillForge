using CoreTestFramework.Core.CrossCuttingConcern.Validation.FluentValidation;
using FluentValidation;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace CoreTestFramework.Core.Aspect.PostSharp
{
    [PSerializable]
    //PostSharp kütüphanesinden gelen OnMethodBoundaryAspect İmplementasyonu sınıfımıza yapıldı.
    public class FluentValidationAspect : OnMethodBoundaryAspect 
    {
        private Type _validatiorType;
        public FluentValidationAspect(Type validatorType)
        {
            _validatiorType = validatorType;
        }
        
        public override void OnEntry(MethodExecutionArgs args)
        {
            
            var validator = (IValidator)Activator.CreateInstance(_validatiorType);
            var entityType =_validatiorType.BaseType.GetGenericArguments()[0];
            var entities = args.Arguments.Where(t => t.GetType() == entityType);

            foreach (var entity in entities)
            {
                ValidatorTool.FluentValidate(validator,entity);
            }
        }
    }
}