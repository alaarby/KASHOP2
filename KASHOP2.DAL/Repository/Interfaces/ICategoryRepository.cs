using KASHOP2.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP2.DAL.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        public List<Category> GetAll();
        public Category Create(Category request);
    }
}
