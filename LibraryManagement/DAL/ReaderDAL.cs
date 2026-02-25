using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DAL
{
    internal class ReaderDAL
    {
        public List<Reader> GetAll()
        {
            List<Reader> list = new List<Reader>();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Readers", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Reader
                    {
                        ReaderId = (int)reader["ReaderId"],
                        FullName = reader["FullName"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Email = reader["Email"].ToString()
                    });
                }
            }

            return list;
        }

        public void Insert(Reader r)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = @"INSERT INTO Readers (FullName, Phone, Email)
                                 VALUES (@FullName, @Phone, @Email)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FullName", r.FullName);
                cmd.Parameters.AddWithValue("@Phone", r.Phone);
                cmd.Parameters.AddWithValue("@Email", r.Email);
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Reader r)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = @"UPDATE Readers SET 
                                 FullName=@FullName,
                                 Phone=@Phone,
                                 Email=@Email
                                 WHERE ReaderId=@ReaderId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ReaderId", r.ReaderId);
                cmd.Parameters.AddWithValue("@FullName", r.FullName);
                cmd.Parameters.AddWithValue("@Phone", r.Phone);
                cmd.Parameters.AddWithValue("@Email", r.Email);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Readers WHERE ReaderId=@ReaderId", conn);
                cmd.Parameters.AddWithValue("@ReaderId", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
