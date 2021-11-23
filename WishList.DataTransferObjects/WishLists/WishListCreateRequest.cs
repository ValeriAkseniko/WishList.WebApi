using System.ComponentModel.DataAnnotations;

namespace WishList.DataTransferObjects.WishLists
{
    public class WishListCreateRequest
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }        
    }
}
