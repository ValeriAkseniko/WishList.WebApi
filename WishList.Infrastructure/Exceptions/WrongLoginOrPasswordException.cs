using System;
using System.Collections.Generic;
using System.Text;

namespace WishList.Infrastructure.Exceptions
{
    public sealed class WrongLoginOrPasswordException : Exception
    {
        public WrongLoginOrPasswordException() : base ($"Wrong login or password")
        {

        }
    }
}
