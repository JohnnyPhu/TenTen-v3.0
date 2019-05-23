using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CategoryDAL
    {
        TenTenDataContext db;
        public CategoryDAL()
        {
            db = new TenTenDataContext();
        }

        public List<Category> getAll()
        {
            List<Category> list = new List<Category>();
            list = db.Categories.Where(x=>x.CategoryID!=4 && x.CategoryID!=6).ToList<Category>();
        
            return list;
        }
    }
}
