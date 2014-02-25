using System.Data.Entity;
//todo: async database work http://msdn.microsoft.com/en-us/data/jj819165
//http://www.asp.net/mvc/tutorials/getting-started-with-ef-using-mvc/async-and-stored-procedures-with-the-entity-framework-in-an-asp-net-mvc-application
//http://blogs.msdn.com/b/webdev/archive/2013/11/01/tutorial-series-updated-for-entity-framework-6-code-first-with-mvc-5.aspx read related data
using FeatureLoggerService.Entities;

namespace FeatureLoggerService.Repositories
{
    public class ModifyContext : DbContext
    {
        public ModifyContext() : base("Name=ModifyContext")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<ModificationInfo> FeatureModifies { get; set; }
        public DbSet<SemanticsModificationInfo> FeatureModyDetails { get; set; }
    }
}
