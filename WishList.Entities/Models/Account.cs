using System;
using System.Collections.Generic;
using System.Text;
using WishList.Entities.Models;

namespace WishList.Entities
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string HashPassword { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; }

        public string Email { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
