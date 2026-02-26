using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
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

        public DataTable LayDanhSachQuaHan() => dal.GetDanhSachQuaHan();
    }
}