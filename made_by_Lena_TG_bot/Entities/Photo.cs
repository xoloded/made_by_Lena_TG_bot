using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace made_by_Lena_TG_bot.Entities
{
    public class Photo
    {
        public long Id { get; set; }
        public string Path { get; set; }
        public List<Product> Products { get; set; }
    }
}
