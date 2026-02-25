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
        public BookView()
        {
            InitializeComponent();
        }

        BookBUS bookBUS = new BookBUS();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBooks();
        }

        private void LoadBooks()
        {
            dgBooks.ItemsSource = bookBUS.GetAll();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtAuthor.Text))
            {
                MessageBox.Show("Không được để trống");
                return;
            }

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Quantity phải >= 0");
                return;
            }

            Book book = new Book
            {
                Title = txtTitle.Text,
                Author = txtAuthor.Text,
                Quantity = quantity,
                AvailableQuantity = quantity
            };

            bookBUS.Add(book);
            LoadBooks();
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgBooks.SelectedItem as Book;
            if (selected == null) return;

            int newQuantity = int.Parse(txtQuantity.Text);
            int borrowed = selected.Quantity - selected.AvailableQuantity;

            if (newQuantity < borrowed)
            {
                MessageBox.Show("Không thể giảm vì đang có sách được mượn");
                return;
            }

            selected.Title = txtTitle.Text;
            selected.Author = txtAuthor.Text;
            selected.Quantity = newQuantity;
            selected.AvailableQuantity = newQuantity - borrowed;

            bookBUS.Update(selected);
            LoadBooks();
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selected = dgBooks.SelectedItem as Book;
            if (selected == null) return;

            if (selected.AvailableQuantity < selected.Quantity)
            {
                MessageBox.Show("Không thể xóa vì đang có người mượn");
                return;
            }

            bookBUS.Delete(selected.Id);
            LoadBooks();
        }
  
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dgBooks.ItemsSource = bookBUS.Search(txtSearch.Text);
        }
    }
}
