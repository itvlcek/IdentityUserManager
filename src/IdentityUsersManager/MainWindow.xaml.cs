using IdentityUsersManager.Logic;
using IdentityUsersManager.Model;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace IdentityUsersManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("cs-CZ");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("cs-CZ");
            LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                        XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            LoginWindow login = new LoginWindow();
            if (login.ShowDialog().Value)
            {
                InitializeComponent();
                UserManager.Users = UserManager.GetAllUsers();
                UsersListBox.ItemsSource = UserManager.Users;
            }
            else
            {
                Close();
            }
        }        

        public void RefreshItems()
        {
            UsersListBox.ClearValue(ItemsControl.ItemsSourceProperty);
            UsersListBox.ItemsSource = UserManager.Users;
        }

        private void NewUserItem_Click(object sender, RoutedEventArgs e)
        {
            var result = new NewUserWindow().ShowDialog();
            if (result != null && result.Value)
            {
                RefreshItems();               
            }
        }

        private void UsersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var user = (ApplicationUser)UsersListBox.SelectedItem;
            if (user != null)
            {
                UserControl.Init(user, this);
            }
        }

        private void TextBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = ((TextBox)sender).Text;
            if (string.IsNullOrEmpty(text))
            {
                UsersListBox.ItemsSource = UserManager.Users;
            }
            else
            {
                if (text.Length > 2)
                {
                    text = text.ToLower();
                    UsersListBox.ItemsSource = UserManager.Users.Where(x => x.UserName.ToLower().Contains(text) || x.Email.ToLower().Contains(text)).ToList();
                }
            }
        }

        private void ChangeConnectionItem_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            if (login.ShowDialog().Value)
            {
                InitializeComponent();
                UserManager.Users = UserManager.GetAllUsers();
                UsersListBox.ItemsSource = UserManager.Users;
            }
        }

        private void BulkEditItem_Click(object sender, RoutedEventArgs e)
        {
            var result = new BulkOperationsWindow().ShowDialog();
            if (result != null && result.Value)
            {
                RefreshItems();
            }
        }
    }
}
