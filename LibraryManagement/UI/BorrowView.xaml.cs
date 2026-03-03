using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LibraryManagement.DAL;
using LibraryManagement.Models;

namespace LibraryManagement.UI
{
    public partial class BorrowView : UserControl
    {
        BookDAL bookDAL = new BookDAL();
        ReaderDAL readerDAL = new ReaderDAL();
        BorrowDAL borrowDAL = new BorrowDAL();

        public BorrowView()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            cbReader.ItemsSource = readerDAL.GetAll();
            cbReader.DisplayMemberPath = "ReaderName";
            cbReader.SelectedValuePath = "ReaderId";

            cbBook.ItemsSource = bookDAL.GetAll();
            cbBook.DisplayMemberPath = "Title";
            cbBook.SelectedValuePath = "BookId";

            dgBorrow.ItemsSource = borrowDAL.GetAllBorrow();
        }

        // ================== MƯỢN ==================
        private void Borrow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbReader.SelectedValue == null || cbBook.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn người đọc và sách!");
                    return;
                }

                if (dpDueDate.SelectedDate == null)
                {
                    MessageBox.Show("Vui lòng chọn hạn trả!");
                    return;
                }

                int readerId = (int)cbReader.SelectedValue;
                int bookId = (int)cbBook.SelectedValue;
                DateTime dueDate = dpDueDate.SelectedDate.Value;

                borrowDAL.InsertBorrow(readerId, bookId, dueDate);

                MessageBox.Show("Mượn sách thành công!");

                ResetForm();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // ================== TRẢ ==================
        private void Return_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgBorrow.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn bản ghi cần trả!");
                    return;
                }

                BorrowRecord record = (BorrowRecord)dgBorrow.SelectedItem;

                if (record.Status == "Returned")
                {
                    MessageBox.Show("Sách đã được trả rồi!");
                    return;
                }

                if (MessageBox.Show("Bạn có chắc muốn trả sách?",
                                    "Xác nhận",
                                    MessageBoxButton.YesNo,
                                    MessageBoxImage.Question)
                                    == MessageBoxResult.No)
                {
                    return;
                }

                borrowDAL.ReturnBook(record.BorrowId);

                MessageBox.Show("Trả sách thành công!");

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // ================== RESET ==================
        private void ResetForm()
        {
            cbReader.SelectedIndex = -1;
            cbBook.SelectedIndex = -1;
            dpDueDate.SelectedDate = null;
        }

        // ================== PLACEHOLDER ==================
        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == "Tìm theo tên người đọc hoặc sách...")
            {
                txtSearch.Text = "";
                txtSearch.Foreground = Brushes.Black;
            }
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Tìm theo tên người đọc hoặc sách...";
                txtSearch.Foreground = Brushes.Gray;
            }
        }

        // ================== SEARCH ==================
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearch.Foreground == Brushes.Gray)
                return;

            string keyword = txtSearch.Text.ToLower();

            dgBorrow.ItemsSource = borrowDAL.GetAllBorrow()
                .Where(x =>
                    x.ReaderName.ToLower().Contains(keyword) ||
                    x.BookTitle.ToLower().Contains(keyword))
                .ToList();
        }
    }
}