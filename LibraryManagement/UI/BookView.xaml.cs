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
using LibraryManagement.BUS;
using LibraryManagement.Models;
namespace LibraryManagement.UI
{
    /// <summary>
    /// Interaction logic for BookView.xaml
    /// </summary>
    public partial class BookView : UserControl
    {
        private string role;
        BookBUS bus = new BookBUS();
        public BookView(string role)
        {
            InitializeComponent();
            this.role = role;

            ApplyPermission();
            LoadData();
        }
        void LoadData()
        {
            dgBooks.ItemsSource = bus.GetAll();
        }

        void ApplyPermission()
        {
            bool isUser = string.Equals(role, "User", StringComparison.OrdinalIgnoreCase);

            if (isUser)
            {
                btnAdd.Visibility = Visibility.Collapsed;
                btnUpdate.Visibility = Visibility.Collapsed;
                btnDelete.Visibility = Visibility.Collapsed;

                txtTitle.IsReadOnly = true;
                txtAuthor.IsReadOnly = true;
                txtCategory.IsReadOnly = true;
                txtQuantity.IsReadOnly = true;

                Brush readOnlyBackground = new SolidColorBrush(Color.FromRgb(241, 245, 249));
                txtTitle.Background = readOnlyBackground;
                txtAuthor.Background = readOnlyBackground;
                txtCategory.Background = readOnlyBackground;
                txtQuantity.Background = readOnlyBackground;

                dgBooks.IsReadOnly = true;
            }
        }

        bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtAuthor.Text) ||
                string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                MessageBox.Show("Không được để trống!");
                return false;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Quantity phải >= 0");
                return false;
            }

            return true;
        }

        void ClearForm()
        {
            txtTitle.Clear();
            txtAuthor.Clear();
            txtCategory.Clear();
            txtQuantity.Clear();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (role == "User")
            {
                MessageBox.Show ("Bạn không có quyền thêm!");
                return;
            }

            if (!ValidateInput()) return;

            int quantity = int.Parse (txtQuantity.Text);

            Book book = new Book()
            {
                Title = txtTitle.Text,
                Author = txtAuthor.Text,
                Category = txtCategory.Text,
                Quantity = quantity,
                AvailableQuantity = quantity
            };

            bus.Add(book);
            MessageBox.Show("Thêm sách thành công!");
            LoadData();
            ClearForm();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

            if (role == "User")
            {
                MessageBox.Show("bạn không có quyền sửa!");
                return;
            }
            if (dgBooks.SelectedItem == null){
                MessageBox.Show ("Chọn sách để sửa!");
                return;
            }

            if (!ValidateInput())
                return;
            Book book = (Book)dgBooks.SelectedItem;

            int newQuantity = int.Parse(txtQuantity.Text);

            if (newQuantity < (book.Quantity - book.AvailableQuantity))
            {
                MessageBox.Show ("Quantity không thể nhỏ hơn số lượng sách đang mượn! ");
                return;

            }
            book.Title = txtTitle.Text;
            book.Author = txtAuthor.Text;
            book.Category = txtCategory.Text;

            int borrowed = book.Quantity - book.AvailableQuantity;
            book.Quantity = newQuantity;
            book.AvailableQuantity = newQuantity - borrowed;

            bus.Update(book);

            MessageBox.Show("Cập nhật thành công!");
            LoadData();
            ClearForm();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

            if (role == "User")
            {
                MessageBox.Show ("Bạn không có quyền xóa!");
                return;
            }
            if (dgBooks.SelectedItem == null){
                MessageBox.Show("Chọn sách để xóa!");
                return;
            }

            Book book = (Book)dgBooks.SelectedItem;

            if (book.AvailableQuantity < book.Quantity)
            {
                MessageBox.Show("Không thể xóa sách đang được mượn");
                return;
            }

            bus.Delete(book.BookId);

            MessageBox.Show ("Xóa thành công!");
            LoadData();
            ClearForm();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                LoadData();
                return;
            }
            dgBooks.ItemsSource = bus.Search(keyword);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
            ClearForm();
            LoadData();
            dgBooks.SelectedItem = null;

            MessageBox.Show("Đã tải lại dữ liệu!");
        }
        private void dgBooks_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgBooks.SelectedItem == null || !(dgBooks.SelectedItem is Book))
            {
                return;
            }

            Book book = (Book)dgBooks.SelectedItem;
            txtTitle.Text = book.Title;
            txtAuthor.Text = book.Author;
            txtCategory.Text = book.Category;
            txtQuantity.Text = book.Quantity.ToString();
        }
    }
}
