using KASHOP2.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.DAL.Utils
{
    public class UserSeedData : ISeedData
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserSeedData(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task DataSeed()
        {
            if(! await _userManager.Users.AnyAsync())
            {
                var user1 = new ApplicationUser
                {
                    UserName = "arabaya",
                    Email = "alaa@gmail.com",
                    FullName = "alaa rabaya",
                    EmailConfirmed = true,
                };
                var user2 = new ApplicationUser
                {
                    UserName = "imosmar",
                    Email = "islam@gmail.com",
                    FullName = "islam mosmar",
                    EmailConfirmed = true,
                };
                var user3 = new ApplicationUser
                {
                    UserName = "orabaya",
                    Email = "oday@gmail.com",
                    FullName = "oday rabaya",
                    EmailConfirmed = true,
                };

                await _userManager.CreateAsync(user1, "Pass@1212");
                await _userManager.CreateAsync(user2, "Pass@1212");
                await _userManager.CreateAsync(user3, "Pass@1212");

                await _userManager.AddToRoleAsync(user1, "SuperAdmin");
                await _userManager.AddToRoleAsync(user2, "Admin");
                await _userManager.AddToRoleAsync(user3, "User");
            }
        }
    }
}
