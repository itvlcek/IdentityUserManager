using IdentityUsersManager.Logic;
using IdentityUsersManager.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace IdentityUsersManager
{
    /// <summary>
    /// Interaction logic for UserControl.xaml
    /// </summary>
    public partial class UserUserControl : UserControl
    {
        ApplicationUser User { get; set; }
        
        public UserUserControl()
        {
            InitializeComponent();
        }

        MainWindow Window { get; set; }

        public void Init(ApplicationUser user, MainWindow window)
        {
            Window = window;
            User = user;

            listboxrole.ItemsSource = UserManager.GetAllRoles().Select(x => new UserRolesModel { Selected = User.Roles.Any(y => y.RoleId == x.Id), RoleId = x.Id }).ToList();

            txtEmailChange.Text = txtEmailReset.Text = User.Email;
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            var result = UserManager.ChangePassword(txtEmailChange.Text, txtPasswordChange.Password, txtPasswordChangeNew.Password);
            if (result.Succeeded)
            {
                MessageBox.Show("Heslo změněno", "Heslo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(string.Format("Chyba při změně hesla :{0}", string.Join(",", result.Errors)), "Heslo", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        private void btnChangeRole_Click(object sender, RoutedEventArgs e)
        {
            var newRoles = new List<string>();
            foreach (UserRolesModel item in listboxrole.Items)
            {
                if(item.Selected)
                    newRoles.Add(item.RoleId);
            }
            UserManager.AddRoles(User, newRoles);

            MessageBox.Show("Role nastaveny", "Role", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            var result = UserManager.ChangePassword(txtEmailReset.Text, txtPasswordResetNew.Password);
            if (result.Succeeded)
            {
                MessageBox.Show("Heslo resetováno", "Heslo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(string.Format("Při resetování hesla se vyskytla chyba :{0}", string.Join(",", result.Errors)), "Heslo", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        
    }


    public class UserRolesModel
    {
        public string RoleId { get; set; }
        public bool Selected { get; set; }
    }
}

