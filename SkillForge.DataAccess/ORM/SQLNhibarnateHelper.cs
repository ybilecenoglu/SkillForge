// using SkillForge.Core.DataAccess.Nhibarnate;
// using FluentNHibernate.Cfg;
// using FluentNHibernate.Cfg.Db;
// using NHibernate;
// using System;
// using System.Collections.Generic;
// using System.Configuration;
// using System.Linq;
// using System.Reflection;
// using System.Text;
// using System.Threading.Tasks;

// namespace SkillForge.Northwind.DataAccess.ORM
// {
//     public class SQLNhibarnateHelper : NhibarnateHelper
//     {
//         protected override ISessionFactory InitializeFactory()
//         {
//             var connstring = ConfigurationManager.ConnectionStrings["NorthwindContext"].ConnectionString;
//             //return SessionFactory
//             return Fluently.Configure().Database(PostgreSQLConfiguration.PostgreSQL83.ConnectionString(connstring)).Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly())).BuildSessionFactory();
//         }
//     }
// }
