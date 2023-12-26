using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DbSeeder
    {
        private UserManager<ApplicationUser> _userManager;

        public DbSeeder(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            var users = new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    UserName = "usera@mail.com",
                    Name = "User A",
                    Email = "usera@mail.com",
                    Tokens = 5
                },
                new ApplicationUser()
                {
                    UserName = "userb@mail.com",
                    Name = "User B",
                    Email = "userb@mail.com",
                    Tokens = 4
                },
                new ApplicationUser()
                {
                    UserName = "userc@mail.com",
                    Name = "User C",
                    Email = "userc@mail.com",
                    Tokens = 5
                },
                new ApplicationUser()
                {
                    UserName = "userd@mail.com",
                    Name = "User D",
                    Email = "userd@mail.com",
                    Tokens = 6
                },
            };
            
                foreach (var user in users)
                {
                    var currentUser = await _userManager.FindByNameAsync(user.Email);
                    if (currentUser == null)
                    {
                        try
                    {

                            var result = await _userManager.CreateAsync(user, "Default@123");
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine("Error in user seeding:", ex.Message);
                        }
                    }
                }
            }
            
        }
    }

