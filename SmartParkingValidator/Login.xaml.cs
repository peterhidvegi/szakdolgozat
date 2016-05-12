using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
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
using Validator.src;

namespace Validator
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        private MainWindow mainWindow;
        private MySqlCommand command;
        private MySqlConnection mysqlConn;
        private NetworkConnection networkConn;
        private DataTable dataTable = new DataTable();
        private User user;
        private const string quary = "SELECT * FROM  `user`";
        private byte[] key = System.Text.Encoding.UTF8.GetBytes("väŁ|ĐáT0r" + Math.PI*Math.E);
        HMACSHA1 hmacSHA1;
        private BackgroundWorker worker = new BackgroundWorker();
        string errorMessage = "";
        


        public Login()
        {
            InitializeComponent();
            mysqlConn = App.getEPSMySqlConnection();
            hmacSHA1 = new HMACSHA1(key);
            logo.Source = new BitmapImage(new Uri(App.mainPath + "\\logo.png"));

            serverName.Text = App.nameFTP;
            serverPass.Password = App.passFTP;
        }


        private bool tryToFindUser()
        {
            bool foundCorrectUser = false;
            int i = 0;
            while (!foundCorrectUser && i < dataTable.Rows.Count)
            {
                if (dataTable.Rows[i][1].ToString() == user.Name && dataTable.Rows[i][2].ToString() == passToStringWithSHA1(loginPass.Password))
                {
                    user.Id = int.Parse(dataTable.Rows[i][0].ToString());
                    foundCorrectUser = true;
                }  
                i++;
            }
            return foundCorrectUser;
        }

        private void MainWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
            if(!mainWindow.IsVisible)
            {
                this.Visibility = Visibility.Visible;
            }else
            {
                mainWindow.StopMotionThreads();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private void signupButton_Click(object sender, RoutedEventArgs e)
        {
            refreshUserDataTable(quary);

            new DbHandler(loginName.Text, passToStringWithSHA1(loginPass.Password));
        }

        private void refreshUserDataTable(string quary)
        {
            if (mysqlConn.State != ConnectionState.Open)
                mysqlConn.Open();

            command = new MySqlCommand(quary, mysqlConn);
            MySqlDataReader queryCommandReader = command.ExecuteReader();
            dataTable.Load(queryCommandReader);
            mysqlConn.Close();
        }

        private string passToStringWithSHA1(string pass)
        {
            var password = System.Text.Encoding.UTF8.GetBytes(pass);

            byte[] saltedHash = hmacSHA1.ComputeHash(password);

            return Convert.ToBase64String(saltedHash);
        }

        private void logIn()
        {
            mainWindow = new MainWindow(user);
            mainWindow.Show();
            mainWindow.IsVisibleChanged += MainWindow_IsVisibleChanged;
            this.Visibility = Visibility.Collapsed;
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                refreshUserDataTable(quary);
                user = new User(loginName.Text, loginPass.Password);

                if (tryToFindUser())
                {
                    logIn();
                }
                else
                {
                    if(errorMessage!="")
                    {
                        MessageBox.Show(errorMessage, "Connection error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }else
                    {
                        MessageBox.Show("Incorrect password or name!", "Connection error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    
                }
            }
            catch (Exception error)
            {

                MessageBox.Show(error.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}

