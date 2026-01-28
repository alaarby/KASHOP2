using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.DAL.DTOs.Responses
{
    public class UserDetailsResponse
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsBlocked { get; set; }
        public List<string> Roles { get; set; }
    }
}
