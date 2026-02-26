using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data; // 1. Đảm bảo có dòng này để hết đỏ chữ DataTable
using LibraryManagement.DAL;

namespace LibraryManagement.BUS
{
    public class ThongKeBUS
    {
        ThongKeDAL dal = new ThongKeDAL();

        public int LayTongSoSach() => dal.GetTongSoSach();
        public string LaySachMuonNhieuNhat() => dal.GetSachMuonNhieuNhat();
        public int LayTongNguoiMuon() => dal.GetTongNguoiMuon();
        public string LayNguoiMuonNhieuNhat() => dal.GetNguoiMuonNhieuNhat();

        // 2. Sửa lại chỗ này cho khớp với tên hàm trong lớp DAL
        public DataTable LayDanhSachQuaHan() => dal.GetDanhSachQuaHan();
    }
}