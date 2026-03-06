using System;
using System.Windows;
using System.Windows.Controls;
using LibraryManagement.Models;
using LibraryManagement.DAL;
using System.Linq;

namespace LibraryManagement.UI
{
    public partial class ReaderView : UserControl
    {
        // Khởi tạo đối tượng DAL để làm việc với Database
        ReaderDAL service = new ReaderDAL();

        public ReaderView()
        {
            InitializeComponent();
            LoadData();
        }

        // Tải danh sách độc giả lên DataGrid
        private void LoadData()
        {
            try
            {
                dgReaders.ItemsSource = service.GetAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải dữ liệu: {ex.Message}", "Lỗi hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Xóa trắng các ô nhập liệu
        private void ClearInput()
        {
            txtName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            dgReaders.SelectedItem = null;
            txtName.Focus();
        }

        // Xử lý sự kiện Thêm độc giả
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên độc giả!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var r = new Reader
                {
                    FullName = txtName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                };

                service.Insert(r);
                MessageBox.Show("Thêm độc giả thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                LoadData();
                ClearInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Xử lý sự kiện Xóa độc giả
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgReaders.SelectedItem is Reader r)
            {
                var confirm = MessageBox.Show(
                    $"Bạn có chắc muốn xóa độc giả: {r.FullName}?",
                    "Xác nhận xóa",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (confirm == MessageBoxResult.Yes)
                {
                    try
                    {
                        service.Delete(r.ReaderId);
                        LoadData();
                        ClearInput();
                        MessageBox.Show("Đã xóa dữ liệu thành công!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi thực thi: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng trong danh sách để xóa.");
            }
        }

        // Xử lý sự kiện Cập nhật thông tin độc giả
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (dgReaders.SelectedItem is Reader r)
            {
                try
                {
                    r.FullName = txtName.Text.Trim();
                    r.Email = txtEmail.Text.Trim();
                    r.Phone = txtPhone.Text.Trim();
                    r.Address = txtAddress.Text.Trim();

                    service.Update(r);
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadData();
                    ClearInput();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi cập nhật: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn độc giả cần sửa từ danh sách!");
            }
        }

        // Xử lý sự kiện Tìm kiếm
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa để tìm kiếm!");
                return;
            }

            var results = service.Search(keyword);
            dgReaders.ItemsSource = results;

            if (results == null || !results.Any())
            {
                MessageBox.Show("Không tìm thấy độc giả nào!");
            }
        }

        // Làm mới danh sách
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
            LoadData();
            ClearInput();
        }

        // Khi chọn một dòng trên DataGrid, đưa dữ liệu lên các ô TextBox
        private void dgReaders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgReaders.SelectedItem is Reader r)
            {
                txtName.Text = r.FullName;
                txtEmail.Text = r.Email;
                txtPhone.Text = r.Phone;
                txtAddress.Text = r.Address;
            }
        }
    }
}