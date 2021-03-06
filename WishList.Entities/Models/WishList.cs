using System;
using System.Collections.Generic;

namespace WishList.Entities.Models
{
    public class WishList
    {
        public Guid Id { get; set; }

        public Guid OwnerId { get; set; }
        public Profile Owner { get; set; }

        public ICollection<ListItem> ListItems { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }

        public WishList()
        {
            ListItems = new List<ListItem>();
        }
    }
}
