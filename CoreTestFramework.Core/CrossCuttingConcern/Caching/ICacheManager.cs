namespace CoreTestFramework.Core.CrossCuttingConcern.Caching
{
    //Output caching uygulayacağımız cache repository
    public interface ICacheManager {
        T Get<T> (string key); //Output cache uygulayacağımız için göndereceğimiz method ve parametreye göre cache bilgisini getirecek method
        object Get(string key);
        void Add(string key, object data, int durationTime); //Cache ekleme yaparken uygulayacağımız patterin için key, data ve cache time bilgisini veriyoruz.
        bool IsAdd(string key); //Eklemek istediğimiz cache daha önce eklenmişmi kontrol ediyoruz.
        void Remove(object key); //Cache datası silkmek için kullanacağımız method
        //void RemoveByPattern(string pattern); //Eğer oluşturduğumuz pattern göre bir cache silme işlemi
        void RemoveByPattern(string pattern);
    }
}