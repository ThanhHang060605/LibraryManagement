using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DAL
{
    internal class BorrowDAL
    {
        public void InsertBorrow(int readerId, int bookId, DateTime dueDate)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string query = @"INSERT INTO BorrowRecords
                                (ReaderId, BookId, BorrowDate, DueDate, Status)
                                VALUES (@ReaderId, @BookId, GETDATE(), @DueDate, 'Borrowing')";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ReaderId", readerId);
                cmd.Parameters.AddWithValue("@BookId", bookId);
                cmd.Parameters.AddWithValue("@DueDate", dueDate);

                cmd.ExecuteNonQuery();

                SqlCommand update = new SqlCommand(
                    "UPDATE Books SET AvailableQuantity = AvailableQuantity - 1 WHERE BookId=@BookId",
                    conn);
                update.Parameters.AddWithValue("@BookId", bookId);
                update.ExecuteNonQuery();
            }
        }

        public void ReturnBook(int borrowId)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                string query = @"UPDATE BorrowRecords
                                 SET ReturnDate = GETDATE(),
                                     Status = 'Returned'
                                 WHERE BorrowId = @BorrowId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@BorrowId", borrowId);
                cmd.ExecuteNonQuery();
            }
        }

        public List<BorrowRecord> GetAllBorrow()
        {
            List<BorrowRecord> list = new List<BorrowRecord>();

            string sql = @"
        SELECT 
            b.BorrowId,
            r.FullName AS ReaderName,
            bk.Title AS BookTitle,
            b.BorrowDate,
            b.DueDate,
            b.Status
        FROM BorrowRecords b
JOIN Readers r ON b.ReaderId = r.ReaderId
JOIN Books bk ON b.BookId = bk.BookId
    ";

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new BorrowRecord
                    {
                        BorrowId = (int)reader["BorrowId"],
                        ReaderName = reader["ReaderName"].ToString(),
                        BookTitle = reader["BookTitle"].ToString(),
                        BorrowDate = (DateTime)reader["BorrowDate"],
                        DueDate = (DateTime)reader["DueDate"],
                        Status = reader["Status"].ToString()
                    });
                }
            }

            return list;
        }
    }
}
