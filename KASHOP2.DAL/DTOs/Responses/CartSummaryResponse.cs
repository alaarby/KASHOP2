using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.DAL.DTOs.Responses
{
    public class CartSummaryResponse
    {
        public List<CartResponse> Items { get; set; }
        public decimal CartTotla => Items.Sum(i => i.TotalPrice);
    }
}
