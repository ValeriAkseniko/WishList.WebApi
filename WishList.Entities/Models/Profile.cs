using System;
using System.Collections.Generic;
using System.Text;

namespace WishList.Entities.Models
{
    public class Profile
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public Profile User { get; set; }

        public string Nickname { get; set; }
        public string Gender { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
