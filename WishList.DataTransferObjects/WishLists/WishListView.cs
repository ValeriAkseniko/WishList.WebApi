using System;
using System.Collections.Generic;
using System.Text;
using WishList.DataTransferObjects.WishListItems;

namespace WishList.DataTransferObjects.WishLists
{
    public class WishListView
    {
        public Guid Id { get; set; }
        public ICollection<WishListItemView> ListItems { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
