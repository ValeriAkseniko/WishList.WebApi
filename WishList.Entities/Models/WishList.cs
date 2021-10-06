using System;
using System.Collections.Generic;
using System.Text;

namespace WishList.Entities.Models
{
    public class WishList
    {
        public Guid Id { get; set; }

        public Guid OwnerId { get; set; }
        public List<Profile> Owner { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
