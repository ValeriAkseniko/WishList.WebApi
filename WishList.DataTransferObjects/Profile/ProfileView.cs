using System;
using WishList.Infrastructure.Constants;

namespace WishList.DataTransferObjects.Profile
{
    public class ProfileView
    {
        public Guid Id { get; set; }
        public Guid? AccountId { get; set; }
        public string Nickname { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
