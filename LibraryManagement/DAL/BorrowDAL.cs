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

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM BorrowRecords", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new BorrowRecord
                    {
                        BorrowId = (int)reader["BorrowId"],
                        ReaderId = (int)reader["ReaderId"],
                        BookId = (int)reader["BookId"],
                        BorrowDate = (DateTime)reader["BorrowDate"],
                        DueDate = (DateTime)reader["DueDate"],
                        ReturnDate = reader["ReturnDate"] as DateTime?,
                        Status = reader["Status"].ToString()
                    });
                }
            }

            return list;
        }
    }
}
