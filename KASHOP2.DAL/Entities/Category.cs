using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.DAL.Entities
{
    public class Category : BaseModel
    {
        public List<Product> Products { get; set; }
        public ICollection<CategoryTranslation> Translations { get; set; } = new List<CategoryTranslation>();
    }
}
