using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.DAL.DTOs.Requests
{
    public class CategoryRequest
    {
        public List<CategoryTranslationRequest> Translations { get; set; }
    }
}
