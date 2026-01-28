using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.DAL.DTOs.Responses
{
    public class UserListResponse
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsBlocked { get; set; }
        public List<string> Roles { get; set; }
    }
}
