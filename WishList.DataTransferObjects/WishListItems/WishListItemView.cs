using System;

namespace WishList.DataTransferObjects.WishListItems
{
    public class WishListItemView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public bool Received { get; set; }
        public decimal? Price { get; set; }
    }
}
