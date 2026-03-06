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
                        Email = reader["Email"].ToString(),
                        Address = reader["Address"].ToString()   // Address
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

                string query = @"INSERT INTO Readers (FullName, Phone, Email, Address)
                                 VALUES (@FullName, @Phone, @Email, @Address)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@FullName", r.FullName);
                cmd.Parameters.AddWithValue("@Phone", r.Phone);
                cmd.Parameters.AddWithValue("@Email", r.Email);
                cmd.Parameters.AddWithValue("@Address", r.Address); // Address

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
                                 Email=@Email,
                                 Address=@Address
                                 WHERE ReaderId=@ReaderId";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@ReaderId", r.ReaderId);
                cmd.Parameters.AddWithValue("@FullName", r.FullName);
                cmd.Parameters.AddWithValue("@Phone", r.Phone);
                cmd.Parameters.AddWithValue("@Email", r.Email);
                cmd.Parameters.AddWithValue("@Address", r.Address); // Address

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

        public List<Reader> Search(string keyword)
        {
            List<Reader> list = new List<Reader>();
            string query = "SELECT * FROM Readers WHERE FullName LIKE @key OR Email LIKE @key OR Phone LIKE @key";

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@key", "%" + keyword + "%");

                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        list.Add(new Reader
                        {
                            ReaderId = Convert.ToInt32(dr["ReaderId"]),
                            FullName = dr["FullName"].ToString(),
                            Email = dr["Email"].ToString(),
                            Phone = dr["Phone"].ToString(),
                            Address = dr["Address"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
    }
}