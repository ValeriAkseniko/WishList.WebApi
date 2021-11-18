using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WishList.DataTransferObjects.Profile
{
    public class ProfileUpdateRequest
    {
        [Required]
        public string Nickname { get; set; }
        public int Gender { get; set; }
        public DateTime Birthday { get; set; }
    }
}
