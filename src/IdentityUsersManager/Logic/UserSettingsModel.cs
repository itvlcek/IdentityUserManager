using System.Collections.Generic;
using System.Xml.Serialization;

namespace IdentityUsersManager.Logic
{
    public class UserSettingsModel
    {
        public UserSettingsModel()
        {
            Servers = new List<ServerModel>();
        }

        public List<ServerModel> Servers { get; set; }
    }

    public class ServerModel
    {
        public ServerModel()
        {
            Databases = new List<DatabaseModel>();
        }

        public string Name { get; set; }
        public List<DatabaseModel> Databases { get; set; }
    }

    public class DatabaseModel
    {
        public DatabaseModel()
        {
            Users = new List<UserModel>();
        }

        public string Name { get; set; }
        public List<UserModel> Users { get; set; }
    }

    public class UserModel
    {
        public string UserName { get; set; }
        public string Salt { get; set; }
        public string Password { get; set; }

        [XmlIgnore]
        public string UnHashPassword
        {
            get
            {
                return new PasswordManager().UnProtectData(Password, Salt);
            }
        }
    }
}
