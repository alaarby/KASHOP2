using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.DAL.DTOs.Responses
{
    public class CheckoutResponse : BaseResponse
    {
        public string? Url { get; set; }
        public string? PaymentId { get; set; }
    }
}
