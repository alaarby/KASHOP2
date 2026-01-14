using KASHOP2.DAL.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.DAL.DTOs.Requests
{
    public class ResetPasswordRequest 
    {
        public string Code { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
