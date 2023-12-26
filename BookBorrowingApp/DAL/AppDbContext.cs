using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Book>()
                .HasOne(book => book.LentByUser)
                .WithMany()
                .HasForeignKey(user => user.LentByUserId);

            builder.Entity<Book>()
                .HasOne(book => book.BorrowedByUser)
                .WithMany()
                .HasForeignKey(b => b.BorrowedByUserId);
                
        }

    }
}
