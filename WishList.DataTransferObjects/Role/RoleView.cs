using System;
using System.Collections.Generic;
using System.Text;

namespace WishList.DataTransferObjects.Role
{
    public class RoleView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
