using System;
using System.Collections.Generic;
using System.Text;

namespace WishList.DataTransferObjects.WishListItems
{
    public class WishListItemCreateRequest
    {
        public Guid WishListId { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
    }
}
