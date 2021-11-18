using System.ComponentModel.DataAnnotations;

namespace WishList.DataTransferObjects.WishListItems
{
    public class WishListItemCreateRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Reference { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
    }
}
