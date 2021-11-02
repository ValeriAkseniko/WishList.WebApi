using System;
using System.Collections.Generic;
using System.Text;

namespace WishList.DataTransferObjects.WishLists
{
    public class WishListCreateRequest
    {
        public Guid OwnerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }        
    }
}
