using System;
using System.Collections.Generic;
using System.Text;

namespace WishList.Entities.Models
{
    public class ListItem
    {
        public Guid Id { get; set; }
        public Guid WishListId { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string Received { get; set; }
        public decimal? Price { get; set; }
    }
}
