using IdentityUsersManager.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IdentityUsersManager
{
    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class RoleDialog : Window
    {
        public RoleDialog(string question)
        {
            InitializeComponent();
            lblQuestion.Content = question;
            listboxrole.ItemsSource = UserManager.GetAllRoles().Select(x => new UserRolesModel { Selected = false , RoleId = x.Id }).ToList();
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
        }

        public List<string> Roles
        {
            get
            {
                var newRoles = new List<string>();
                foreach (UserRolesModel item in listboxrole.Items)
                {
                    if (item.Selected)
                        newRoles.Add(item.RoleId);
                }
                return newRoles;
            }
        }
    }
}
