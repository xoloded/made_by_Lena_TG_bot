using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace made_by_Lena_TG_bot.Entities
{
    public enum ProductCategory
    {
        None,
        Косметичка,
        Шоппер,
        РезиночкаДляВолос,
        Гирлянда
    }
    public class Category
    {
        public long Id { get; set; }
        public ProductCategory ProductCategory { get; set; }   

        public List<Product> Products { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
