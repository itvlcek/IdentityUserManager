using System.Windows;

namespace IdentityUsersManager
{
    /// <summary>
    /// Interaction logic for NewUserWindow.xaml
    /// </summary>
    public partial class NewUserWindow : Window
    {
        public NewUserWindow()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var result = Logic.UserManager.CreateNew(txtEmail.Text, txtPassword.Password);

            if (result.Succeeded)
            {
                MessageBox.Show("Uživatel založen", "Uživatel", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show(string.Format("Chyba při založení uživatele :{0}", string.Join(",", result.Errors)), "Uživatel", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }
    }
}
