using System;
using System.Data;
using System.Data.SqlClient;

namespace LibraryManagement.DAL
{
    public class ThongKeDAL
    {
        string strCon = @"Data Source=.\SQLEXPRESS;Initial Catalog=LibraryDB;Integrated Security=True";

        // ================= TỔNG SỐ SÁCH =================
        public int GetTongSoSach()
        {
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Books", conn);
                return (int)cmd.ExecuteScalar();
            }
        }

        // ================= SÁCH MƯỢN NHIỀU NHẤT =================
        public string GetSachMuonNhieuNhat()
        {
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                conn.Open();

                string sql = @"SELECT TOP 1 b.Title
                               FROM BorrowRecords br
                               JOIN Books b ON br.BookId = b.BookId
                               GROUP BY b.Title
                               ORDER BY COUNT(*) DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                object result = cmd.ExecuteScalar();

                return result != null ? result.ToString() : "N/A";
            }
        }

        // ================= TỔNG NGƯỜI MƯỢN =================
        public int GetTongNguoiMuon()
        {
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                conn.Open();

                string sql = @"SELECT COUNT(DISTINCT ReaderId) 
                               FROM BorrowRecords";

                SqlCommand cmd = new SqlCommand(sql, conn);
                return (int)cmd.ExecuteScalar();
            }
        }

        // ================= NGƯỜI MƯỢN NHIỀU NHẤT =================
        public string GetNguoiMuonNhieuNhat()
        {
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                conn.Open();

                string sql = @"SELECT TOP 1 r.FullName
                               FROM BorrowRecords br
                               JOIN Readers r ON br.ReaderId = r.ReaderId
                               GROUP BY r.FullName
                               ORDER BY COUNT(*) DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                object result = cmd.ExecuteScalar();

                return result != null ? result.ToString() : "N/A";
            }
        }

        // ================= SÁCH QUÁ HẠN =================
        public DataTable GetDanhSachQuaHan()
        {
            using (SqlConnection conn = new SqlConnection(strCon))
            {
                conn.Open();

                string sql = @"SELECT 
                                b.BookId AS MaSach,
                                b.Title AS TenSach,
                                r.FullName AS TenDocGia,
                                br.DueDate AS NgayHenTra
                               FROM BorrowRecords br
                               JOIN Books b ON br.BookId = b.BookId
                               JOIN Readers r ON br.ReaderId = r.ReaderId
                               WHERE br.Status = 'Borrowing'
                               AND br.DueDate < GETDATE()";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);

                DataTable dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
        }
    }
}