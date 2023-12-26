using Domain.Dtos;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBookServices
    {
        public Task<BookDto> Add(BookDto book);
        public BookDto GetById(int bookId);
        public Task<BookDto> Borrow(int bookId, string borrowerId);
        public BookDto ReturnBook(int bookId);
        public List<BookDto> GetAll();
        public List<BookDto> GetAllLented(string userEmail);
        public List<BookDto> GetAllBorrowed(string userEmail);
    }
}
