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
using LibraryManagement.DAL;

namespace LibraryManagement.UI
{
    /// <summary>
    /// Interaction logic for DashboardAdminView.xaml
    /// </summary>
    public partial class DashboardAdminView : UserControl
    {
        ThongKeDAL tk = new ThongKeDAL();
        public DashboardAdminView()
        {
            InitializeComponent();
            LoadDashboard();
        }
        private void LoadDashboard()
        {
            txtTongSoSach.Text = tk.GetTongSoSach().ToString();
            txtTongNguoiMuon.Text = tk.GetTongNguoiMuon().ToString();
            txtSachMuonNhieu.Text = tk.GetSachMuonNhieuNhat();
            txtNguoiMuonNhieu.Text = tk.GetNguoiMuonNhieuNhat();
            dgSachQuaHan.ItemsSource = tk.GetDanhSachQuaHan().DefaultView;
        }
    }
}
