using System;
using System.Collections.Generic;

namespace webquanlydouong.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }  // Mật khẩu không mã hóa
        public bool EmailVerified { get; set; }

        // Role của người dùng (Admin, User, Manager, v.v.)
        public string Role { get; set; }

        // Mối quan hệ với đơn hàng (Orders)
        public virtual ICollection<Order> Orders { get; set; }
    }
}
