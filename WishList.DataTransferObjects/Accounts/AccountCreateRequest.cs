using System.ComponentModel.DataAnnotations;

namespace WishList.DataTransferObjects.Accounts
{
    public class AccountCreateRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        [MinLength(4)]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
