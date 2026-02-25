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
using LibraryManagement.Models;

namespace LibraryManagement.UI
{
    public partial class LoginWindow : Window
    {
        private List<Account> accounts;

        public LoginWindow()
        {
            InitializeComponent();
            LoadAccounts();
        }

        private void LoadAccounts()
        {
            accounts = new List<Account>
            {
                new Account { Username = "admin", Password = "12345", Role = "Admin" },
                new Account { Username = "user", Password = "12345", Role = "User" }
            };
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            var account = accounts
                .FirstOrDefault(a => a.Username == username && a.Password == password);

            if (account != null)
            {
                MessageBox.Show("Login successful!");

                MainWindow main = new MainWindow(account.Role);
                main.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password!");
            }
        }
    }
}