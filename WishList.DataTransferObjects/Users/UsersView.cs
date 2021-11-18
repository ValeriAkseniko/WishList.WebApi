using System;

namespace WishList.DataTransferObjects.Users
{
    public class UsersView
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string Nickname { get; set; }
        public string Gender { get; set; }
    }
}
