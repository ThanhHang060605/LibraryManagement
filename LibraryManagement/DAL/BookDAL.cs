using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.DAL
{
    internal class BookDAL
    {
        public List<Book> GetAll()
        {
            List<Book> list = new List<Book>();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Books";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Book
                    {
                        BookId = (int)reader["BookId"],
                        Title = reader["Title"].ToString(),
                        Author = reader["Author"].ToString(),
                        Category = reader["Category"].ToString(),
                        Quantity = (int)reader["Quantity"],
                        AvailableQuantity = (int)reader["AvailableQuantity"]
                    });
                }
            }

            return list;
        }

        public void Insert(Book book)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = @"INSERT INTO Books 
                                (Title, Author, Category, Quantity, AvailableQuantity)
                                VALUES (@Title, @Author, @Category, @Quantity, @AvailableQuantity)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@Author", book.Author);
                cmd.Parameters.AddWithValue("@Category", book.Category);
                cmd.Parameters.AddWithValue("@Quantity", book.Quantity);
                cmd.Parameters.AddWithValue("@AvailableQuantity", book.AvailableQuantity);

                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Book book)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = @"UPDATE Books SET 
                                Title=@Title,
                                Author=@Author,
                                Category=@Category,
                                Quantity=@Quantity,
                                AvailableQuantity=@AvailableQuantity
                                WHERE BookId=@BookId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@BookId", book.BookId);
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@Author", book.Author);
                cmd.Parameters.AddWithValue("@Category", book.Category);
                cmd.Parameters.AddWithValue("@Quantity", book.Quantity);
                cmd.Parameters.AddWithValue("@AvailableQuantity", book.AvailableQuantity);

                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM Books WHERE BookId=@BookId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@BookId", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
