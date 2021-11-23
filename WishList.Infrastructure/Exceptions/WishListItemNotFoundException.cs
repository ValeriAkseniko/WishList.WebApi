using System;
using System.Collections.Generic;
using System.Text;

namespace WishList.Infrastructure.Exceptions
{
    public sealed class WishListItemNotFoundException : Exception
    {
        public Guid? Id { get; set; }
        public WishListItemNotFoundException(Guid id) : base($"{id} WishList item not found")
        {
            Id = id;
            Data.Add(nameof(id), id);
        }
        public override string ToString()
        {
            var message = string.Empty;
            if (Id != null)
            {
                message = $"{Id} WishList item not found";
            }
            return message;
        }
    }
}
