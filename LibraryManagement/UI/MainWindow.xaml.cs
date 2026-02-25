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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LibraryManagement.UI;
namespace LibraryManagement.UI
{
    public partial class MainWindow : Window
    {
        private string role;

        public MainWindow(string role)
        {
            InitializeComponent();
            this.role = role;
            SetupRole();
        }

        private void SetupRole()
        {
            txtRole.Text = "Logged in as: " + role;

            if (role == "User")
            {
                btnManageBooks.Visibility = Visibility.Collapsed;
                btnDashboard.Visibility = Visibility.Collapsed;
            }
        }

        private void btnManageBooks_Click(object sender, RoutedEventArgs e)
        {
            BookView bookView = new BookView();
            bookView.Show();
            this.Hide();
        }
    }
}