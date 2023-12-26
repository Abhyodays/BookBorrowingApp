using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
namespace Domain.Interfaces
{
    public interface IBookRepository
    {
        public Book GetById(int id);
        public Book Add(Book book);
        public Book Borrow(Book book, ApplicationUser borrower);
        public Book ReturnBook(int bookId);
        public List<Book> GetAll();
        public List<Book> GetAllLented(string userEmail);
        public List<Book> GetAllBorrowed(string userEmail);

    }
}
