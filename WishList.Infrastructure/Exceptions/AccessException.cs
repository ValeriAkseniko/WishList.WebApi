using System;

namespace WishList.Infrastructure.Exceptions
{
    public sealed class AccessException : Exception
    {
        public AccessException() : base($"Not enough rights")
        {

        }
    }
}
