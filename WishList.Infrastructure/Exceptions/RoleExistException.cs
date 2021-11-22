using System;
using System.Collections.Generic;
using System.Text;

namespace WishList.Infrastructure.Exceptions
{
    public sealed class RoleExistException : Exception
    {
        public Guid Id { get; set; }

        public RoleExistException(Guid id) : base($"{id} role not found")
        {
            Id = Id;
            Data.Add(nameof(id), id);
        }
        public override string ToString()
        {
            var message = string.Empty;
            if (Id != null)
            {
                message = $"{Id} role not found";
            }
            return message;
        }
    }
}
