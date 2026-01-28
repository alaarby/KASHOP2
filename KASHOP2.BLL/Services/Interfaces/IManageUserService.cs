using KASHOP2.DAL.DTOs.Requests;
using KASHOP2.DAL.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.BLL.Services.Interfaces
{
    public interface IManageUserService
    {
        Task<List<UserListResponse>> GetUsersAsync();
        Task<UserDetailsResponse> GetUserDetailsAsync();
        Task<BaseResponse> BlockedUserAsync(string userId);
        Task<BaseResponse> UnBlockedUserAsync(string userId);
        Task<BaseResponse> ChangeUserRoleAsync(ChangeUserRoleRequest request);
    }
}
