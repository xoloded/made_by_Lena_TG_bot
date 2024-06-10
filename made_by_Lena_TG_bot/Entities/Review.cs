using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace made_by_Lena_TG_bot.Entities
{
    public class Review
    {
        public long Id { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
        public long CategoryId { get; set; }

        public string UserName { get; set; }
        public DateTime DataTime { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
        public long ProductId { get; set; }

        public string Text { get; set; }
        public int Rating { get; set; }

        [ForeignKey(nameof(PhotoId))]
        public Photo Photo { get; set; }
        public long PhotoId { get; set; }
    }
}