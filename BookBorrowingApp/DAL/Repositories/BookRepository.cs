using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class BookRepository : IBookRepository
    {
        private AppDbContext _context;
        private ILogger<BookRepository> _logger;
        private UserManager<ApplicationUser> _userManager;
        public BookRepository(AppDbContext context, ILogger<BookRepository> logger, UserManager<ApplicationUser>userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;

        }
        public List<Book> GetAll()
        {
            return _context.Books.Include(book => book.LentByUser).Include(book => book.BorrowedByUser).ToList();
        }
        public Book Add(Book book)
        {
            try
            {
                var existingBook = _context.Books.Include(book => book.LentByUser).Include(book => book.BorrowedByUser).FirstOrDefault(b => b.Name == book.Name && b.Author == book.Author);
                if (existingBook != null) throw new Exception("Book with same name and author already exist.");
                _context.Books.Add(book);
                _context.SaveChanges();
                return book; 
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Error occurred while adding a book to the database.");
                throw;
            }
        }

        public Book Borrow(Book book, ApplicationUser borrower)
        {
            try 
            {
                book.BorrowedByUserId = borrower.Id;
                book.LentByUser.Tokens++;
                book.IsAvailable = false;
                borrower.Tokens--;
                _context.SaveChanges();
                return book;
                
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while borrowing book.");
                throw;
            }
        }

        public Book GetById(int id)
        {
            try
            {
                return _context.Books.Include(book => book.LentByUser).Include(book => book.BorrowedByUser).FirstOrDefault(b => b.Id == id);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while finding a book in the database.");
                throw;
            }
        }

        public Book ReturnBook(int bookId)
        {
            try
            {
                var book = _context.Books.Include(book => book.LentByUser).Include(book => book.BorrowedByUser).FirstOrDefault(b => b.Id == bookId);
                if(book == null)
                {
                    throw new Exception("Book with given Id not exist.");
                }
                book.IsAvailable = true;
                book.BorrowedByUserId = null;
                _context.SaveChanges();
                return book;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occured while returning book.");
                throw;
            }
        }

        public List<Book> GetAllLented(string userEmail)
        {
            return _context.Books.Include(book => book.LentByUser).Include(book => book.BorrowedByUser).Where(b => b.LentByUser.Email == userEmail).ToList();
        }

        public List<Book> GetAllBorrowed(string userEmail)
        {
            return _context.Books.Include(book => book.LentByUser).Include(book => book.BorrowedByUser).Where(b => b.BorrowedByUser.Email == userEmail).ToList();
        }
    }
}
