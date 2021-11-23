using System;
using System.Collections.Generic;
using System.Text;

namespace WishList.Infrastructure.Exceptions
{
    public sealed class RoleNotFoundException : Exception
    {
        public Guid Id { get; set; }

        public RoleNotFoundException(Guid id) : base($"{id} role not found")
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
