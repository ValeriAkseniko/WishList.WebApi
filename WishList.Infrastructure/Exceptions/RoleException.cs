using System;

namespace WishList.Infrastructure.Exceptions
{
    public sealed class RoleException : Exception
    {
        public RoleException() : base($"Role rights error")
        {

        }
    }
}
