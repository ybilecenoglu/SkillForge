using System.Linq.Expressions;

namespace SkillForge.Core.Utilities
{
    //Amaç: Expression tree'leri parse edip filtreleri çıkarmak.
    public static class ExpressionFilterParser
    {
        /*
            static class: yardımcı (utility) sınıf. Her yerden ExpressionFilterParser.ExtractFilters şeklinde çağırılır.
            Girdi: LambdaExpression — örneğin p => p.category_id == categoryID && p.supplier_id == supplierID.

            Dikkat: LambdaExpression tipinden kullanıyoruz ki generic tip (T) bağımlılığı olmasın. Yani Expression<Func<Product,bool>> veya Expression<Func<Category,bool>> her ikisi de LambdaExpression olarak alınabilir.
        */
        public static Dictionary<string, object> ExtractFilters(LambdaExpression expression)
        {
            var result = new Dictionary<string, object>(); //var result = new Dictionary<string, object>(); → sonuçları koyacağımız dict.
            if (expression == null) return result; //if (expression == null) return result; → ifade yoksa boş dict dön.

            ParseExpression(expression.Body, result); // ParseExpression(expression.Body, result); → Lambda’ın gövdesini parse et. LambdaExpression.Body genelde BinaryExpression veya UnaryExpression vs olur.
            return result; // return result; → filtre adı -> değer dictionary’si döndürülür.
        }

        //Bu metod recursive (özyinelemeli) parse yapar.
        private static void ParseExpression(Expression expr, Dictionary<string, object> result) //BinaryExpression genelde a == b, a > b, a && b gibi iki tarafa sahip ifadeler için kullanılır.
        {
            if (expr is BinaryExpression binary)
            {
                if (binary.NodeType == ExpressionType.Equal) //eşitlik kontrolü (==) ile karşılaşırsak:
                {
                    if (binary.Left is MemberExpression member) //sol taraf bir MemberExpression ise (çoğunlukla p.CategoryId gibi) o zaman
                    {
                        var value = GetValue(binary.Right); //sağ tarafın runtime değerini al.
                        result[member.Member.Name] = value; //member ismi (örn. CategoryId) ile değeri dict’e koy.
                    }
                }
                else if (binary.NodeType == ExpressionType.AndAlso || binary.NodeType == ExpressionType.OrElse) //Bu durumda sol ve sağ alt ifadeye tekrar ParseExpression çağırılır — yani a && b && c şeklindeki ifadeler derinlemesine ayrıştırılır.
                {
                    ParseExpression(binary.Left, result);
                    ParseExpression(binary.Right, result);
                }
            }
        }

        //Expression’ın gerçek (runtime) değerini almak.
        private static object GetValue(Expression expr)
        {
            if (expr is ConstantExpression constExpr) //Eğer ifade ConstantExpression ise (örn. 42 veya "abc"), kolayca constExpr.Value al.
            {
                return constExpr.Value;
            }

            //Expression'i unique şekilde key'e çeviriyoruz
            var cacheKey = expr.ToString();

            var objectMember = Expression.Convert(expr, typeof(object)); //sağlanan expr'i (hangi tip olursa olsun) object'e convert eden bir ifade oluşturuyoruz. (Tip uyumu vs. için)
            var getterLambda = Expression.Lambda<Func<object>>(objectMember); //bu objectMember ifadesinden parametresiz bir Func<object> lambda’ı oluşturuyoruz (yani "bu ifadeyi çalıştır ve object döndür" diyoruz).
            var getter = getterLambda.Compile(); //expression tree’den gerçek bir delegate üretiyoruz (derleme).
            return getter(); //delegate’i çağırıyoruz ve runtime değeri alıyoruz (örn. 5).
        }
    }
}