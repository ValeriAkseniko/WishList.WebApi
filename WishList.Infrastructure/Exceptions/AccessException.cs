using System;

namespace WishList.Infrastructure.Exceptions
{
    public sealed class AccessException : Exception
    {
        public AccessException() : base($"The vishlist doesn't belong to you")
        {

        }
    }
}
