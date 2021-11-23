using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WishList.DataTransferObjects.WishListItems
{
    public class WishListItemsCreateRequest
    {
        [Required]
        public Guid WishListId { get; set; }
        public List<WishListItemCreateRequest> WishListItems { get; set; }
    }
}
