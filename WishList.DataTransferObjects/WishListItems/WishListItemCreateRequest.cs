using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WishList.DataTransferObjects.WishListItems
{
    public class WishListItemCreateRequest
    {
        [Required]
        public Guid WishListId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Reference { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
    }
}
