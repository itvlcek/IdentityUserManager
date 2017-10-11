using IdentityUsersManager.Logic;
using System.Windows;
using System.Linq;
using System;

namespace IdentityUsersManager
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private UserSettingsModel Settings { get; set; }
        private SettingsManager SettingsManager { get; set; }
        public LoginWindow()
        {
            InitializeComponent();
            SettingsManager = new SettingsManager();
            Settings = SettingsManager.LoadUserSettings();
            Settings.Servers.ForEach(x => { comboBoxServer.Items.Add(x.Name); });
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (checkBoxRemember.IsChecked.Value)
            {
                SettingsManager.SaveSettings(txtPassword.Password, comboBoxLogin.Text, comboBoxServer.Text, comboBoxDatabase.Text);
            }
            SettingsManager.Login = comboBoxLogin.Text;
            SettingsManager.Server = comboBoxServer.Text;
            SettingsManager.Database = comboBoxDatabase.Text;
            SettingsManager.Password = txtPassword.Password;

            try
            {
                UserManager.GetAllRoles();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nepodařilo se navázat spojení", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        private void comboBoxDatabase_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (comboBoxDatabase.SelectedValue != null && comboBoxServer.SelectedValue != null)
            {
                comboBoxLogin.Items.Clear();
                (Settings.Servers.FirstOrDefault(x => x.Name == comboBoxServer.SelectedValue.ToString())?.Databases.FirstOrDefault(x => x.Name == comboBoxDatabase.SelectedValue.ToString())?.Users ?? new System.Collections.Generic.List<UserModel>()).ForEach(x => { comboBoxLogin.Items.Add(x.UserName); });

                if (comboBoxLogin.Items.Count > 0)
                    comboBoxLogin.SelectedItem = comboBoxLogin.Items[0];
            }
            
        }

        private void comboBoxLogin_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (comboBoxDatabase.SelectedValue != null && comboBoxServer.SelectedValue != null && comboBoxLogin.SelectedValue != null)
                txtPassword.Password = (Settings.Servers.FirstOrDefault(x => x.Name == comboBoxServer.SelectedValue.ToString())?.Databases.FirstOrDefault(x => x.Name == comboBoxDatabase.SelectedValue.ToString())?.Users ?? new System.Collections.Generic.List<UserModel>()).FirstOrDefault(x => x.UserName == comboBoxLogin.SelectedValue.ToString())?.UnHashPassword ?? "";

        }

        private void comboBoxServer_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            if (comboBoxServer.SelectedValue != null)
            {
                comboBoxDatabase.Items.Clear();
                (Settings.Servers.FirstOrDefault(x => x.Name == comboBoxServer.SelectedValue.ToString())?.Databases ?? new System.Collections.Generic.List<DatabaseModel>()).ForEach(x => { comboBoxDatabase.Items.Add(x.Name); });
                if (comboBoxDatabase.Items.Count > 0)
                    comboBoxDatabase.SelectedItem = comboBoxDatabase.Items[0];
            }
        }
    }
}
