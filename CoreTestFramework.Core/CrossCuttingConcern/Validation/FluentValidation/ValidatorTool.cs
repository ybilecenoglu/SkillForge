using FluentValidation;

namespace CoreTestFramework.Core.CrossCuttingConcern.Validation.FluentValidation
{
    public class ValidatorTool {
        //Fluent validation gelen IValidator sınıfı ve neyi validate etmek istediğimi bildireceğim object entity methodun içersine parametre olarak gönderip refactoring yaparak genel bir yapı elde ediyorum.
        public static void FluentValidate(IValidator validator, object entity){

            var context = new ValidationContext<object>(entity);
            var result = validator.Validate(context);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}