using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Book name is required field.")]
        [MaxLength(150, ErrorMessage = "Name can maximum 150 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Rating is required field.")]
        [Range(0, 5, ErrorMessage = "Rating must be between 0 and 5.")]
        public double Rating { get; set; }
        [Required(ErrorMessage = "Author is required field.")]
        [MaxLength(100, ErrorMessage = "Author can maximum 100 characters.")]
        public string Author { get; set; }
        [Required(ErrorMessage = "Genre is required field.")]
        [MaxLength(100, ErrorMessage = "Genre can maximum 100 characters.")]
        public string Genre { get; set; }
        public bool IsAvailable { get; set; }
        [Required(ErrorMessage = "Description is required field.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Lent by user can not be null.")]
        public string LentByUserId { get; set; }
        public string BorrowedByUserId { get; set; }
    }
}
