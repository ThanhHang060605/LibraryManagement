using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using LibraryManagement.Models;

namespace LibraryManagement.DAL
{
    class BookDAL
    {
        string connectionString =
@"Server=DESKTOP-J6FHSRU\SQLEXPRESS;
Database=LibraryDB;
Trusted_Connection=True;";
        public List<Book> GetAll()
        {
            List<Book> list = new List<Book>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Books";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Book
                    {
                        Id = (int)reader["BookId"],
                        Title = reader["Title"].ToString(),
                        Author = reader["Author"].ToString(),
                        Quantity = (int)reader["Quantity"],
                        AvailableQuantity = (int)reader["AvailableQuantity"]
                    });
                }
            }

            return list;
        }

        public void Insert(Book book)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Books 
                        (Title, Author, Quantity, AvailableQuantity)
                        VALUES (@Title,@Author,@Quantity,@AvailableQuantity)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@Author", book.Author);
                cmd.Parameters.AddWithValue("@Quantity", book.Quantity);
                cmd.Parameters.AddWithValue("@AvailableQuantity", book.AvailableQuantity);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Book book)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Books SET 
                        Title=@Title,
                        Author=@Author,
                        Quantity=@Quantity,
                        AvailableQuantity=@AvailableQuantity
                        WHERE BookId=@Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Id", book.Id);
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@Author", book.Author);
                cmd.Parameters.AddWithValue("@Quantity", book.Quantity);
                cmd.Parameters.AddWithValue("@AvailableQuantity", book.AvailableQuantity);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Books WHERE BookId=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Book> Search(string keyword)
        {
            List<Book> list = new List<Book>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT * FROM Books 
                         WHERE Title LIKE @key OR Author LIKE @key";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@key", "%" + keyword + "%");

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Book
                    {
                        Id = (int)reader["Id"],
                        Title = reader["Title"].ToString(),
                        Author = reader["Author"].ToString(),
                        Quantity = (int)reader["Quantity"],
                        AvailableQuantity = (int)reader["AvailableQuantity"]
                    });
                }
            }

            return list;
        }
    }
}
