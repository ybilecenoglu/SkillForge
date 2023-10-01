using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreTestFramework.Core.DataAccess.Nhibarnate
{
    public abstract class NhibarnateHelper : IDisposable
    {
        //Entitiy freamwork context benzeri kullanıcı hangi veritabanıyla geldiğini anlamak için aldığımız sesion değişkeni
        private static ISessionFactory _sessionFactory;
        
        //Kullanıcıdan gelen sessionfactory aldık.
        public virtual ISessionFactory SessionFactory { get { return _sessionFactory ?? (_sessionFactory = InitializeFactory()); } }

        //Implemente edilecek yerlerde hangi veritabanı ile geleceği belli olmadığı için methodu ezilebilir hale getirdik.
        protected abstract ISessionFactory InitializeFactory();

        //Gelen session açmak için kullandığımız method.
        public virtual ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
