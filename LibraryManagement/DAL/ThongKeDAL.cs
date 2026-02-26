using System;
using System.Data;
using System.Data.SqlClient;

namespace LibraryManagement.DAL
{
    public class ThongKeDAL
    {
        // Kiểm tra kĩ chuỗi kết nối này
        string strCon = @"Data Source=.\SQLEXPRESS;Initial Catalog=LibraryDB;Integrated Security=True";

        public int GetTongSoSach()
        {
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Sach", conn);
                return (int)cmd.ExecuteScalar();
            }
        }

        public string GetSachMuonNhieuNhat()
        {
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                conn.Open();
                string sql = "SELECT TOP 1 TenSach FROM MuonTra GROUP BY TenSach ORDER BY COUNT(*) DESC";
                SqlCommand cmd = new SqlCommand(sql, conn);
                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : "N/A";
            }
        }

        public int GetTongNguoiMuon()
        {
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                conn.Open();
                string sql = "SELECT COUNT(DISTINCT TenDocGia) FROM MuonTra";
                SqlCommand cmd = new SqlCommand(sql, conn);
                return (int)cmd.ExecuteScalar();
            }
        }

        public string GetNguoiMuonNhieuNhat()
        {
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                conn.Open();
                string sql = "SELECT TOP 1 TenDocGia FROM MuonTra GROUP BY TenDocGia ORDER BY COUNT(*) DESC";
                SqlCommand cmd = new SqlCommand(sql, conn);
                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : "N/A";
            }
        }

        public DataTable GetDanhSachQuaHan()
        {
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                conn.Open();
                string sql = "SELECT MaSach, TenSach, TenDocGia, NgayHenTra FROM MuonTra WHERE NgayHenTra < GETDATE() AND TrangThai = N'Chưa trả'";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
    }
}