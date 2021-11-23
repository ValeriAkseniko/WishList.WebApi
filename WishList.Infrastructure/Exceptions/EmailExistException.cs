using System;
using System.Collections.Generic;
using System.Text;

namespace WishList.Infrastructure.Exceptions
{
    public sealed class EmailExistException : Exception
    {
        public string Email { get; set; }

        public EmailExistException(string email) : base($"{email} already exist")
        {
            Email = email;
            Data.Add(nameof(email), email);
        }
        public override string ToString()
        {
            var message = string.Empty;
            if (Email != null)
            {
                message = $"{Email} already exist";
            }

            return message;
        }
    }
}
