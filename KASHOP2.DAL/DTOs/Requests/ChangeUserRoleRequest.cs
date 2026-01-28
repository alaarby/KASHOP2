using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.DAL.DTOs.Requests
{
    public class ChangeUserRoleRequest
    {
        public string UserId { get; set; }
        public string Role { get; set; }
    }
}
