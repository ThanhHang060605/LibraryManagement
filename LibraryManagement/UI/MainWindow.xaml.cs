using LibraryManagement.DAL;
using System;
using System.Windows;
using LibraryManagement.UI;

namespace LibraryManagement.UI
{
    public partial class MainWindow : Window
    {
        private string role;
        private int currentReaderId;

        public MainWindow(string role, int readerId)
        {
            InitializeComponent();
            this.role = role;
            this.currentReaderId = readerId;

            SetupRole();

            // Mặc định hiển thị danh sách sách khi vừa vào
            MainContent.Content = new BookView(role);
        }

        private void SetupRole()
        {
            txtRole.Text = "Logged in as: " + role;

            // Phân quyền: Nếu là User thì ẩn các chức năng quản lý của Admin
            if (role == "User")
            {
                btnManageReaders.Visibility = Visibility.Collapsed;
                btnDashboard.Visibility = Visibility.Collapsed;
            }
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new DashboardAdminView();
        }

        private void btnManageBooks_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new BookView(role);
        }

        private void btnManageReaders_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ReaderView();
        }

        private void btnManageBorrow_Click(object sender, RoutedEventArgs e)
        {
            // Truyền role và ID độc giả hiện tại vào màn hình Mượn/Trả
            MainContent.Content = new BorrowView(role, currentReaderId);
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                LoginWindow login = new LoginWindow();
                login.Show();
                this.Close();
            }
        }

        // Phương thức bổ trợ để test Database nếu cần (đã comment)
        private void TestDatabase()
        {
            try
            {
                BookDAL dal = new BookDAL();
                dal.Insert(new Models.Book
                {
                    Title = "Test Book",
                    Author = "Admin",
                    Category = "Test",
                    Quantity = 5,
                    AvailableQuantity = 5
                });

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