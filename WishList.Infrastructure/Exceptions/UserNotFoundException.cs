using System;
using System.Collections.Generic;
using System.Text;

namespace WishList.Infrastructure.Exceptions
{
    public sealed class UserNotFoundException : Exception
    {
        public Guid? Id { get; set; }

        public string Login { get; set; }

        public UserNotFoundException(Guid id) : base($"{id} not found")
        {
            Id = Id;
            Data.Add(nameof(id), id);
        }
        public UserNotFoundException(string login) : base($"{login} not found")
        {
            Login = login;
            Data.Add(nameof(login), login);
        }
        public override string ToString()
        {
            var message = string.Empty;
            if (Id != null)
            {
                message = $"{Id} not found";
            }
            if (Login != null)
            {
                message = $"{Login} not found";
            }
            return message;
        }
    }
}
