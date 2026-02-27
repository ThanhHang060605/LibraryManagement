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
    public partial class BookView : Window
    {
        BookBUS bus = new BookBUS();
        public BookView()
        {
            InitializeComponent();
            LoadData();
        }
        void LoadData()
        {
            dgBooks.ItemsSource = bus.GetAll();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtAuthor.Text) ||
                string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                MessageBox.Show("Không được để trống!");
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Quantity phải >= 0");
                return;
            }

            Book book = new Book()
            {
                Title = txtTitle.Text,
                Author = txtAuthor.Text,
                Category = txtCategory.Text,
                Quantity = quantity,
                AvailableQuantity = quantity
            };

            bus.Add(book);
            LoadData();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgBooks.SelectedItem == null) return;

            Book book = (Book)dgBooks.SelectedItem;

            book.Title = txtTitle.Text;
            book.Author = txtAuthor.Text;
            book.Category = txtCategory.Text;
            book.Quantity = int.Parse(txtQuantity.Text);

            bus.Update(book);
            LoadData();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgBooks.SelectedItem == null) return;

            Book book = (Book)dgBooks.SelectedItem;

            if (book.AvailableQuantity < book.Quantity)
            {
                MessageBox.Show("Không thể xóa sách đang được mượn");
                return;
            }

            bus.Delete(book.BookId);
            LoadData();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dgBooks.ItemsSource = bus.Search(txtSearch.Text);
        }

        private void dgBooks_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgBooks.SelectedItem == null) return;

            Book book = (Book)dgBooks.SelectedItem;

            txtTitle.Text = book.Title;
            txtAuthor.Text = book.Author;
            txtCategory.Text = book.Category;
            txtQuantity.Text = book.Quantity.ToString();
        }
    }
}
