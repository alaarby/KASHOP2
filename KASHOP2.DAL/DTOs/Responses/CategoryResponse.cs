using KASHOP2.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.DAL.DTOs.Responses
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        public Status Status { get; set; }
        public List<CategoryTranslationResponse> Ttranslations { get; set; }
    }
}
