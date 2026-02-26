using LibraryManagement.DAL;
using System;
using System.Windows;
// CHỖ NÀY QUAN TRỌNG: Đảm bảo có dòng này để chương trình hiểu DashboardAdminView là gì
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

            TestDatabase(); 
        }

        private void SetupRole()
        {
            txtRole.Text = "Logged in as: " + role;

            if (role == "User")
            {
                btnManageBooks.Visibility = Visibility.Collapsed;
                btnManageReaders.Visibility = Visibility.Collapsed; // Ẩn luôn quản lý độc giả nếu là User
                btnDashboard.Visibility = Visibility.Collapsed;
            }
        }

        // Đây là hàm xử lý sự kiện Click đã khai báo trong XAML
        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            // Nếu dòng dưới vẫn đỏ, hãy nhấn Ctrl + Shift + B để Build lại Project
            DashboardAdminView dashboard = new DashboardAdminView();

            // MainContent phải khớp với x:Name trong XAML bạn vừa gửi
            MainContent.Content = dashboard;
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