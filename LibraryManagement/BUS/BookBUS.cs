using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Models;
using LibraryManagement.DAL;
namespace LibraryManagement.BUS
{
    class BookBUS
    {
        BookDAL dal = new BookDAL();

        public List<Book> GetAll()
        {
            return dal.GetAll();
        }

        public void Add(Book book)
        {
            dal.Insert(book);
        }

        public void Update(Book book)
        {
            dal.Update(book);
        }

        public void Delete(int id)
        {
            dal.Delete(id);
        }
        public List<Book> Search(string keyword)
        {
            return dal.Search(keyword);
        }
    }
}
