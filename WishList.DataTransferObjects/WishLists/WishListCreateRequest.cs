using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WishList.DataTransferObjects.WishLists
{
    public class WishListCreateRequest
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }        
    }
}
