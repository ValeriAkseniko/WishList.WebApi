using System;
using System.ComponentModel.DataAnnotations;

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
