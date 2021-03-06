using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WishList.Entities.Models
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string HashPassword { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public Guid? ProfileId { get; set; }
        [ForeignKey("ProfileId")]
        public Profile Profile { get; set; }

        public string Email { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
