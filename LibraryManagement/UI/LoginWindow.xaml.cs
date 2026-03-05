using LibraryManagement.DAL;
using LibraryManagement.Models;
using System.Linq;
using System.Windows;
using System.Collections.Generic;

namespace LibraryManagement.UI
{
    public partial class LoginWindow : Window
    {
        private List<Account> accounts;
        private ReaderDAL readerDAL = new ReaderDAL();

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
                int readerId = 1; // user demo luôn có ReaderId = 1

                MainWindow main = new MainWindow(account.Role, readerId);
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