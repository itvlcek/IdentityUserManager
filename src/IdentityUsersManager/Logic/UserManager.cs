using IdentityUsersManager.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;

namespace IdentityUsersManager.Logic
{
    public class UserManager
    {

        public static List<ApplicationUser> Users { get; set; }

        public static IdentityResult CreateNew(string email, string password)
        {
            var store = new UserStore<ApplicationUser>(UserEntities.Create());
            var manager = new UserManager<ApplicationUser>(store);
            var user = new ApplicationUser { Email = email, UserName = email, PasswordHash = password };
            var result = manager.Create(user, password);
            Users.Add(user);
            return result;
        }

        public static IdentityResult ChangePassword(string email, string password, string newPassword)
        {
            var store = new UserStore<ApplicationUser>(UserEntities.Create());
            var manager = new UserManager<ApplicationUser>(store);
            var user = manager.FindByName(email);
            return manager.ChangePassword(user.Id, password, newPassword);
        }

        public static IdentityResult ChangePassword(string email, string newPassword)
        {
            var store = new UserStore<ApplicationUser>(UserEntities.Create());
            var manager = new UserManager<ApplicationUser>(store);
            var user = manager.FindByName(email);

            var provider = new DpapiDataProtectionProvider("Sample");

            manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(provider.Create("EmailConfirmation"));

            var token = manager.GeneratePasswordResetToken(user.Id);
            return manager.ResetPassword(user.Id, token, newPassword);
        }

        public static List<ApplicationUser> GetAllUsers()
        {
            var store = new UserStore<ApplicationUser>(UserEntities.Create());
            var manager = new UserManager<ApplicationUser>(store);
            Users = manager.Users.ToList();
            return Users;
        }

        public static ApplicationUser GetUser(string Id)
        {
            var store = new UserStore<ApplicationUser>(UserEntities.Create());
            var manager = new UserManager<ApplicationUser>(store);

            return manager.FindById(Id);
        }

        public static ApplicationUser GetuserByEmail(string email)
        {
            return Users.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
        }

        public static void AddRoles(ApplicationUser user, List<string> roles)
        {
            var store = new UserStore<ApplicationUser>(UserEntities.Create());
            var manager = new UserManager<ApplicationUser>(store);

            foreach (var role in roles)
            {
                if (!user.Roles.Any(x => x.RoleId == role))
                {
                    manager.AddToRole(user.Id, role);
                }
            }
            var missingRoles = user.Roles.Where(x => roles.Contains(x.RoleId) == false);

            foreach (var role in missingRoles)
            {
                manager.RemoveFromRole(user.Id, role.RoleId);
            }
            RefreshUsers();
        }

        public static void AddRole(ApplicationUser user, List<string> roles)
        {
            var store = new UserStore<ApplicationUser>(UserEntities.Create());
            var manager = new UserManager<ApplicationUser>(store);

            foreach (var role in roles)
            {
                if (!user.Roles.Any(x => x.RoleId == role))
                {
                    manager.AddToRole(user.Id, role);
                }
            }           
        }

        public static void RefreshUsers()
        {
            GetAllUsers();
        }

        public static List<IdentityRole> GetAllRoles()
        {
            using (var entities = UserEntities.Create())
            {
                return entities.Roles.ToList();
            }
        }
    }
}
