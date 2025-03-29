using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webquanlydouong.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }  // Có thể là "Chờ xử lý", "Đang giao", "Hoàn thành"
        public virtual User User { get; set; }  // Mối quan hệ với User
        public virtual ICollection<Product> Products { get; set; }  // Mối quan hệ với Products
    }

}
