using System;
using System.Collections.Generic;
using System.Text;

namespace WishList.Infrastructure.Exceptions
{
    public sealed class WishListNotFoundException : Exception
    {
        public Guid Id { get; set; }

        public WishListNotFoundException(Guid id) : base($"{id} WishList not found")
        {
            Id = id;
            Data.Add(nameof(id), id);
        }

        public override string ToString()
        {
            var message = string.Empty;
            if (Id != null)
            {
                message = $"{Id} WishList not found";
            }
            return message;
        }
    }
}
