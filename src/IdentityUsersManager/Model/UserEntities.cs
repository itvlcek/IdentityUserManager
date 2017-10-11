using IdentityUsersManager.Logic;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace IdentityUsersManager.Model
{

    public class UserEntities : IdentityDbContext<ApplicationUser>
    {
        private static string CreateConnectionString()
        {
            return string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};Integrated Security=False", SettingsManager.Server, SettingsManager.Database, SettingsManager.Login, SettingsManager.Password);
        }

        public UserEntities()
            : base(CreateConnectionString())
        {
            Database.SetInitializer<UserEntities>(null);
            Database.CommandTimeout = 10000;
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public static UserEntities Create()
        {
            return new UserEntities();
        }
    }
}
