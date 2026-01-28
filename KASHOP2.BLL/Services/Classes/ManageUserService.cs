using KASHOP2.BLL.Services.Interfaces;
using KASHOP2.DAL.DTOs.Requests;
using KASHOP2.DAL.DTOs.Responses;
using KASHOP2.DAL.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.BLL.Services.Classes
{
    public class ManageUserService : IManageUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageUserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<List<UserListResponse>> GetUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            
            var response = users.Adapt<List<UserListResponse>>();
            for (int i = 0; i < users.Count; i++)
            {
                var roles = await _userManager.GetRolesAsync(users[i]);
                response[i].Roles = roles.ToList();
            }
            return response;
        }
        public async Task<BaseResponse> BlockedUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            await _userManager.UpdateAsync(user);

            return new BaseResponse
            {
                Success = true,
                Message = "user Blocked"
            };

        }
        public async Task<BaseResponse> UnBlockedUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.SetLockoutEnabledAsync(user, false);
            await _userManager.SetLockoutEndDateAsync(user, null);
            await _userManager.UpdateAsync(user);

            return new BaseResponse
            {
                Success = true,
                Message = "user UnBlocked"
            };
        }
        public async Task<BaseResponse> ChangeUserRoleAsync(ChangeUserRoleRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            var currentRules = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, currentRules);
            await _userManager.AddToRoleAsync(user, request.Role);
            return new BaseResponse
            {
                Success = true,
                Message = "role updated"
            };
        }
        public Task<UserDetailsResponse> GetUserDetailsAsync()
        {
            throw new NotImplementedException();
        }
         }
}
