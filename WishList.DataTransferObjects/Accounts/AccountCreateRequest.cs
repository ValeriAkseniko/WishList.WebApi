using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WishList.DataTransferObjects.Accounts
{
    public class AccountCreateRequest
    {        
        [Required]
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
