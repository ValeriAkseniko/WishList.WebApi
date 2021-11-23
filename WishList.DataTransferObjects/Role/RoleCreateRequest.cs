using System.ComponentModel.DataAnnotations;

namespace WishList.DataTransferObjects.Role
{
    public class RoleCreateRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
