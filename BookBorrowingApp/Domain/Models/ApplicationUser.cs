using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage ="Name is required.")]
        [MaxLength(100, ErrorMessage ="User can have name of maximum 100 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Tokens are required.")]
        public int Tokens { get; set; }
    }
}
