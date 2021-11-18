using System;
using System.Collections.Generic;
using System.Text;
using WishList.Entities.Models;

using RoleDb = WishList.Entities.Models.Role;

namespace WishList.DataTransferObjects.Users
{
    public class UsersView
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public RoleDb Role { get; set; }
        public string Nickname { get; set; }
        public Gender Gender { get; set; }
    }
}
