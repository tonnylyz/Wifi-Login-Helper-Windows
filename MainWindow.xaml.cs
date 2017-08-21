using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Windows;
using WifiLoginHelper.Properties;

namespace WifiLoginHelper
{
    /// <summary>
    ///     MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if (username != string.Empty && password != string.Empty)
            {
                Username.Text = username;
                Password.Password = password;
                LoginRequest();
            }
        }

        private static string username
        {
            get { return Settings.Default.username; }
            set
            {
                //var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //config.AppSettings.Settings["username"].Value = value;
                //config.Save(ConfigurationSaveMode.Full, true);
                Settings.Default.username = value;
                Settings.Default.Save();
            }
        }

        private static string password
        {
            get { return Settings.Default.password; }
            set
            {
                //var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //config.AppSettings.Settings["password"].Value = value;
                //config.Save(ConfigurationSaveMode.Full, true);
                Settings.Default.password = value;
                Settings.Default.Save();
            }
        }

        private async void LoginRequest()
        {
            var values = new Dictionary<string, string>
            {
                {"action", "login"},
                {"username", username},
                {"password", "{B}" + Convert.ToBase64String(Encoding.UTF8.GetBytes(password))},
                {"ac_id", "1"},
                {"save_me", "0"},
                {"ajax", "1"}
            };
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync(
                    "https://gw.buaa.edu.cn:801/include/auth_action.php",
                    content);
                var result = await response.Content.ReadAsStringAsync();
                if (result.StartsWith("login_ok"))
                    Snackbar.MessageQueue.Enqueue("Login succeeded.");
                else
                    Snackbar.MessageQueue.Enqueue(result);
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (Username.Text != string.Empty && Password.Password != string.Empty)
            {
                username = Username.Text;
                password = Password.Password;
                LoginRequest();
            }
            else
            {
                Snackbar.MessageQueue.Enqueue("Empty username or password.");
            }
        }
    }
}