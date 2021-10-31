using System;
using System.Collections.Generic;
using System.Text;

namespace WishList.DataTransferObjects.Profile
{
    public class ProfileCreateRequest
    {
        public Guid AccountId { get; set; }
        public string Nickname { get; set; }
        public int Gender { get; set; }
        public DateTime Birthday { get; set; }        
    }
}
