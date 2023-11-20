using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreTestFramework.WebMvcUI.Common
{
    public  static class ConvertSelectListItem {

        // private static ConvertSelectListItem Instance;
        
        // public static ConvertSelectListItem CreateInstance(){
        //     return Instance ?? (Instance = new ConvertSelectListItem());
        // }

        public static SelectList GetSelectList<T> (List<T> entities, string value, string textField, object selectedValue )
        {
            Type type = entities.GetType();
            PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            string deneme = props[0].Name;
            return new SelectList(entities, value, textField, selectedValue);
        }
        public static T GetObject<T>(Dictionary<string,string> dict) 
        {
            //Gelen T nesnenin tipini alıyoruz
            Type type = typeof(T);
            //Gelen nesneden bir instance oluşturuyoruz
            var obje = Activator.CreateInstance(type);
            //Methoda gelen dictionary tipinde aldığımız verileri key ve value olarak obje nesnesine yazıyoruz
            foreach (var keyValue in dict)
            {
                type.GetProperty(keyValue.Key).SetValue(obje, keyValue.Value);
            }

            //Gelen T tipinde obje nesnemizi gönderiyoruz
            return (T)obje;
        }

    }
}