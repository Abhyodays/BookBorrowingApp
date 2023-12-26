using Domain.Dtos;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BookServices : IBookServices
    {
        private IBookRepository _bookRepository;
        private UserManager<ApplicationUser> _userManager;
        public BookServices(IBookRepository bookRepository, UserManager<ApplicationUser> userManager)
        {
            _bookRepository = bookRepository;
            _userManager = userManager;
        }

        private BookDto ToBookDto(Book book)
        {
            if (book == null) return null;
            return new BookDto
            {
                Id= book.Id,
                Name = book.Name,
                Author = book.Author,
                Rating = book.Rating,
                Genre = book.Genre,
                IsAvailable = book.IsAvailable,
                Description = book.Description,
                LentByUserId = book.LentByUser.Email,
                BorrowedByUserId = book.BorrowedByUser?.Email
            };
        }
        private async Task<Book> ToBook(BookDto book)
        {
            if (book == null || book.LentByUserId == null) return null;
            var lentUser = await _userManager.FindByNameAsync(book.LentByUserId);
            var borrowUser = book.BorrowedByUserId != null ? await _userManager.FindByNameAsync(book.BorrowedByUserId) : null;
            return new Book
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                Rating = book.Rating,
                Genre = book.Genre,
                Description = book.Description,
                LentByUserId = book.LentByUserId,
                LentByUser = lentUser,
                BorrowedByUserId = book.BorrowedByUserId,
                BorrowedByUser = borrowUser
            };
        }
        public async Task<BookDto> Add(BookDto book)
        {
            var newBook = await ToBook(book);
            newBook.IsAvailable = true;
            try
            {
                var addedBook = _bookRepository.Add(newBook);
                return ToBookDto(addedBook);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task<BookDto> Borrow(int bookId, string borrowerId)
        {
            var borrower = await _userManager.FindByNameAsync(borrowerId);
            var book = _bookRepository.GetById(bookId);
            if (book == null || book.LentByUserId == borrower.Id || borrower.Tokens <= 0 || !book.IsAvailable) return null;
            return ToBookDto(_bookRepository.Borrow(book, borrower));
        }

        public BookDto GetById(int bookId)
        {
            try
            {
                return ToBookDto(_bookRepository.GetById(bookId));
            }
            catch (Exception)
            {
                throw;
            }
        }
        public BookDto ReturnBook(int bookId)
        {
            try
            {
                return ToBookDto(_bookRepository.ReturnBook(bookId));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BookDto> GetAll()
        {
            var books = _bookRepository.GetAll();
            return books.Select(book => ToBookDto(book)).ToList();
        }

        public List<BookDto> GetAllLented(string userEmail)
        {
            var books = _bookRepository.GetAllLented(userEmail);
            return books.Select(book => ToBookDto(book)).ToList();
        }

        public List<BookDto> GetAllBorrowed(string userEmail)
        {
            var books = _bookRepository.GetAllBorrowed(userEmail);
            return books.Select(book => ToBookDto(book)).ToList();
        }
    }
}
