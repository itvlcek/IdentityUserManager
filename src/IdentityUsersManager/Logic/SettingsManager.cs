using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace IdentityUsersManager.Logic
{
    public class SettingsManager
    {
        public static string Login { get; set; }
        public static string Password { get; set; }
        public static string Server { get; set; }
        public static string Database { get; set; }

        private static UserSettingsModel Settings { get; set; }
        public void SaveSettings(string password, string user, string server, string database)
        {
            var s = Settings.Servers.FirstOrDefault(x => x.Name == server);

            if (s == null)
            {
                s = new ServerModel { Name = server };
                Settings.Servers.Add(s);
            }
            var d = s.Databases.FirstOrDefault(x => x.Name == database);
            if (d == null)
            {
                d = new DatabaseModel { Name = database };
                s.Databases.Add(d);
            }

            var login = d.Users.FirstOrDefault(x => x.UserName == user);

            if (login == null)
            {
                login = new UserModel { UserName = user };
                d.Users.Add(login);
            }
            var salt = "";
            var pass = new PasswordManager().ProtectPassword(password, out salt);

            login.Password = pass;
            login.Salt = salt;
           
            XmlSerializer xs = new XmlSerializer(typeof(UserSettingsModel));
            using (TextWriter tw = new StreamWriter("settings.xml"))
            {
                xs.Serialize(tw, Settings);
            }
        }

        public UserSettingsModel LoadUserSettings()
        {
            XmlSerializer xs = new XmlSerializer(typeof(UserSettingsModel));
            if (File.Exists("settings.xml"))
            {
                using (var sr = new StreamReader("settings.xml"))
                {
                    Settings = (UserSettingsModel)xs.Deserialize(sr);
                }
            }
            else
            {
                Settings = new UserSettingsModel();
            }
            return Settings;
        }     
    }
}
