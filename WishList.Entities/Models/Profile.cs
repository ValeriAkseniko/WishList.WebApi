using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WishList.Entities.Models
{
    public class Profile
    {
        public Guid Id { get; set; }

        public Guid? AccountId { get; set; }
        [ForeignKey("AccountId")]
        public Account Account { get; set; }

        public string Nickname { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }

        public ICollection<WishList> WishLists { get; set; }

        public Profile()
        {
            WishLists = new List<WishList>();
        }
    }
}
