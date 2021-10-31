using System;
using System.Collections.Generic;
using System.Text;

namespace WishList.DataTransferObjects.Accounts
{
    public class AccountCreateRequest
    {        
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
