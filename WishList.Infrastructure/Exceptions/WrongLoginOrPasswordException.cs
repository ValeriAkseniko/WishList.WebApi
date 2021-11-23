using System;

namespace WishList.Infrastructure.Exceptions
{
    public sealed class WrongLoginOrPasswordException : Exception
    {
        public WrongLoginOrPasswordException() : base($"Wrong login or password")
        {

        }
    }
}
