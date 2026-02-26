using LibraryManagement.DAL;
using System;
using System.Windows;

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

            TestDatabase(); 
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

        private void TestDatabase()
        {
            try
            {
                BookDAL dal = new BookDAL();

                // INSERT
                dal.Insert(new Models.Book
                {
                    Title = "Test Book",
                    Author = "Admin",
                    Category = "Test",
                    Quantity = 5,
                    AvailableQuantity = 5
                });

                // SELECT
                var books = dal.GetAll();

                string result = "";
                foreach (var b in books)
                {
                    result += b.BookId + " - " + b.Title + "\n";
                }

                MessageBox.Show(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
    }
}