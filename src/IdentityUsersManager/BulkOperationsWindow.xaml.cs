using IdentityUsersManager.Logic;
using IdentityUsersManager.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace IdentityUsersManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class BulkOperationsWindow : Window
    {
        public BulkOperationsWindow()
        {
            InitializeComponent();           
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            txtInfo.Text = string.Empty;
            var inputDialog = new InputDialog("Zadejte požadované heslo");
            if (inputDialog.ShowDialog() == true)
            {
                var users = GetUsers();

                var password = inputDialog.Answer;
                Task.Run(() =>
                {
                    foreach (var user in users)
                    {
                        if (!UserManager.Users.Any(x => x.Email.ToLower() == user.ToLower()))
                        {
                            var result = UserManager.CreateNew(user, password);
                            txtInfo.Dispatcher.Invoke(new Action(() => txtInfo.Text += (Environment.NewLine + $"Založení uživatele {user} skončilo s výsledkem {result.Succeeded} {string.Join(",", result.Errors)}")));
                        }
                        else
                        {
                            txtInfo.Dispatcher.Invoke(new Action(() => txtInfo.Text += (Environment.NewLine + $"Uživatel {user} již je založen")));
                        }
                    }

                    MessageBox.Show("Uživatelé založeni", "OK", MessageBoxButton.OK);
                });
            }
        }

        private List<string> GetUsers()
        {
            var users = txtData.Text.Split(new string[] { System.Environment.NewLine }, System.StringSplitOptions.None);
            return users.Where(x => x.Contains('@')).Select(x=>x.Trim()).ToList();
        }

        private void btnSetRoles_Click(object sender, RoutedEventArgs e)
        {
            txtInfo.Text = string.Empty;
            var inputDialog = new RoleDialog("Vyberte role");
            if (inputDialog.ShowDialog() == true)
            {
                var users = GetUsers();

                var roles = inputDialog.Roles;
                Task.Run(() =>
                {
                    foreach (var user in users)
                    {
                        var userEntity = UserManager.GetuserByEmail(user);

                        if (userEntity != null)
                        {
                            UserManager.AddRole(userEntity, roles);
                            txtInfo.Dispatcher.Invoke(new Action(() => txtInfo.Text += (Environment.NewLine + $"Role přidány uživateli {user}")));

                        }
                        else
                        {
                            txtInfo.Dispatcher.Invoke(new Action(() => txtInfo.Text += (Environment.NewLine + $"Uživatel {user} není založen")));
                        }
                    }
                    UserManager.RefreshUsers();
                    MessageBox.Show("Přidání rolí dokončeno", "OK", MessageBoxButton.OK);
                });
            }
        }

        private void btnSetPassword_Click(object sender, RoutedEventArgs e)
        {
            txtInfo.Text = string.Empty;
            var inputDialog = new InputDialog("Zadejte požadované heslo");
            if (inputDialog.ShowDialog() == true)
            {
                var users = GetUsers();

                var password = inputDialog.Answer;
                Task.Run(() =>
                {
                    foreach (var user in users)
                    {
                        if (UserManager.Users.Any(x => x.Email.ToLower() == user.ToLower()))
                        {
                            var result = UserManager.ChangePassword(user, password);
                            txtInfo.Dispatcher.Invoke(new Action(() => txtInfo.Text += (Environment.NewLine + $"Heslo uživatele {user} nastaveno")));
                        }
                        else
                        {
                            txtInfo.Dispatcher.Invoke(new Action(() => txtInfo.Text += (Environment.NewLine + $"Uživatel {user} neexistuje")));
                        }
                    }

                    MessageBox.Show("Hesla nastavena", "OK", MessageBoxButton.OK);
                });
            }
        }
    }
}
