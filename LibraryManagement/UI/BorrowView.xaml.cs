using LibraryManagement.DAL;
using LibraryManagement.Models;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LibraryManagement.UI
{
    public partial class BorrowView : UserControl
    {
        BookDAL bookDAL = new BookDAL();
        ReaderDAL readerDAL = new ReaderDAL();
        BorrowDAL borrowDAL = new BorrowDAL();

        private readonly string role;
        private readonly int currentReaderId;

        public BorrowView(string role, int readerId)
        {
            InitializeComponent();
            this.role = role;
            currentReaderId = readerId;
            Loaded += BorrowView_Loaded;
        }

        private void BorrowView_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            SetupRole();
        }

        private void SetupRole()
        {
            if (role == "User")
            {
                // Ẩn chọn người đọc
                spReader.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadData()
        {
            // Load Reader (Admin dùng)
            cbReader.ItemsSource = readerDAL.GetAll();
            cbReader.DisplayMemberPath = "FullName";
            cbReader.SelectedValuePath = "ReaderId";

            // Chỉ hiển thị sách còn số lượng
            cbBook.ItemsSource = bookDAL.GetAll()
                .Where(b => b.AvailableQuantity > 0)
                .ToList();

            cbBook.DisplayMemberPath = "Title";
            cbBook.SelectedValuePath = "BookId";

            dgBorrow.ItemsSource = borrowDAL.GetAllBorrow();
        }

        //================== MƯỢN ==================
        //private void Borrow_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (cbBook.SelectedValue == null)
        //        {
        //            MessageBox.Show("Vui lòng chọn sách!");
        //            return;
        //        }

        //        if (dpDueDate.SelectedDate == null)
        //        {
        //            MessageBox.Show("Vui lòng chọn hạn trả!");
        //            return;
        //        }

        //        int readerId;

        //        if (role == "User")
        //        {
        //            readerId = currentReaderId;
        //        }
        //        else
        //        {
        //            if (cbReader.SelectedValue == null)
        //            {
        //                MessageBox.Show("Vui lòng chọn người đọc!");
        //                return;
        //            }

        //            readerId = (int)cbReader.SelectedValue;
        //        }

        //        int bookId = (int)cbBook.SelectedValue;
        //        DateTime dueDate = dpDueDate.SelectedDate.Value;

        //        borrowDAL.InsertBorrow(readerId, bookId, dueDate);

        //        MessageBox.Show("Mượn sách thành công!");

        //        ResetForm();
        //        LoadData();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
        private void Borrow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbBook.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn sách!");
                    return;
                }

                if (dpDueDate.SelectedDate == null)
                {
                    MessageBox.Show("Vui lòng chọn hạn trả!");
                    return;
                }

                int readerId;

                if (role == "User")
                {
                    readerId = currentReaderId;
                }
                else
                {
                    if (cbReader.SelectedValue == null)
                    {
                        MessageBox.Show("Vui lòng chọn người đọc!");
                        return;
                    }

                    readerId = (int)cbReader.SelectedValue;
                }

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
            //txtReader.Text = "";
            cbReader.SelectedIndex = -1;
            cbBook.SelectedIndex = -1;
            dpDueDate.SelectedDate = null;
        }


        // ================== SEARCH ==================
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!IsLoaded) return;

            if (string.IsNullOrWhiteSpace(txtSearch.Text) ||
                txtSearch.Text == "Tìm theo tên người đọc hoặc sách...")
            {
                dgBorrow.ItemsSource = borrowDAL.GetAllBorrow();
                return;
            }

            string keyword = txtSearch.Text.ToLower();

            if (role == "Admin")
            {
                dgBorrow.ItemsSource = borrowDAL.GetAllBorrow()
                    .Where(x =>
                        (x.ReaderName != null && x.ReaderName.ToLower().Contains(keyword)) ||
                        (x.BookTitle != null && x.BookTitle.ToLower().Contains(keyword)))
                    .ToList();
            }
            else
            {
                // User chỉ tìm theo sách
                dgBorrow.ItemsSource = borrowDAL.GetAllBorrow()
                    .Where(x =>
                        x.BookTitle != null &&
                        x.BookTitle.ToLower().Contains(keyword))
                    .ToList();
            }
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

        private void dgBorrow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}