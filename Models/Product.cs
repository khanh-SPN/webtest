using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webquanlydouong.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }  // Đồ uống, Bánh, Kem
        public decimal Price { get; set; }
        public string Description { get; set; }

        public string ImageUrl { get; set; }  //url hinh ảnh

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }

}
