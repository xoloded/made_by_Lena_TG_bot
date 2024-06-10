using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace made_by_Lena_TG_bot.Entities
{

    public class Product
    {
        public long Id { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
        public long CategoryId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool Available { get; set; }

        //[ForeignKey(nameof(PhotoId))]
        //public Photo Photo { get; set; }
        //public long PhotoId { get; set; }
        public List<Photo> Photos { get; set; }

        public List<Review> Reviews { get; set; }
    }
}
