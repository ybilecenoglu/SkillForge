using FluentValidation; //FluentValidation kütüphanesini kullanıyoruz → validasyon kurallarını ve ValidationContext, ValidationException gibi sınıfları buradan alıyoruz.

namespace CoreTestFramework.Core.CrossCuttingConcern.Validation.FluentValidation
{
    //Herhangi bir entity’yi, dışarıdan verilen bir IValidator ile kontrol etmek.
    public class ValidatorTool
    {
        /*
            static → Her yerden doğrudan çağrılabilir (new’lemeden).

            Parametreler:

            IValidator validator → Dışarıdan hangi validator kullanılacak (ProductValidator, CategoryValidator vs.).

            object entity → Hangi nesne validate edilecek (Product, Category vs.).

            📌 Bu yapı sayesinde generic bir validasyon akışı elde etmiş oluyorsun.
        */
        public static void FluentValidate(IValidator validator, object entity)
        {
            //ValidationContext oluşturuluyor.
            //entity parametresini alıp validasyonun “hedefi” olarak set ediyoruz.
            var context = new ValidationContext<object>(entity);

            //Verilen validator (örn: ProductValidator) context üzerinde çalıştırılıyor.
            //result → Validasyon sonucu (IsValid, Errors gibi bilgiler içerir).
            var result = validator.Validate(context);

            //Eğer validasyon başarısızsa (IsValid == false) → bir ValidationException fırlatıyoruz.
            //Bu exception’da result.Errors yer alıyor → hangi alanların, hangi kuraldan dolayı geçersiz olduğu bilgisi mevcut.
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}