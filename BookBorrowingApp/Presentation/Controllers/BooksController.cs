using Domain.Dtos;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private IBookServices _bookServices;
        public BooksController(IBookServices bookServices)
        {
            _bookServices = bookServices;
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public ActionResult<IEnumerable<BookDto>> GetAllBooks()
        {
            var bookDtos = _bookServices.GetAll();
            return Ok(bookDtos);
        }
        [Authorize]
        [HttpGet("all/lented")]
        public ActionResult<IEnumerable<BookDto>> GetAllLentedBooks(string email)
        {
            var bookDtos = _bookServices.GetAllLented(email);
            return Ok(bookDtos);
        }
        [Authorize]
        [HttpGet("all/borrowed")]
        public ActionResult<IEnumerable<BookDto>> GetAllBorrowedBooks(string email)
        {
            var bookDtos = _bookServices.GetAllBorrowed(email);
            return Ok(bookDtos);
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<ActionResult> AddBook(BookDto book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var addedBook = await _bookServices.Add(book);
            }
            catch(Exception ex)
            {
                return BadRequest(new { Message = ex.Message});
            }
            return Ok(book);
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<BookDto> GetById(int id)
        {
            var book = _bookServices.GetById(id);
            if (book == null)
            {
                return NotFound(new { Message = "No book exists with given Id." });
            }
            return book;
        }
        [HttpPut("borrow")]
        [Authorize]
        public async Task<ActionResult<BookDto>> Borrow(int bookId, string userId)
        {
            if(bookId<0 || userId == null)
            {
                return BadRequest();
            }
            try
            {
                var book = await _bookServices.Borrow(bookId, userId);
                if (book == null)
                {
                    return BadRequest();
                }
                return Ok(book);
            }
            catch(Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpPut("return")]
        [Authorize]
        public ActionResult<BookDto> Return(int bookId)
        {
            try
            {
                var book = _bookServices.ReturnBook(bookId);
                return book;
            }
            catch(Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


    }

}
