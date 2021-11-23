using System;

namespace WishList.Infrastructure.Exceptions
{
    public sealed class WishListByOwnerNotFound : Exception
    {
        public Guid? Id { get; set; }
        public WishListByOwnerNotFound(Guid id) : base($"WishList by owner {id} not found")
        {
            Id = id;
            Data.Add(nameof(id), id);
        }

        public override string ToString()
        {
            var message = string.Empty;
            if (Id != null)
            {
                message = $"WishList by owner {Id} not found";
            }
            return message;
        }
    }
}
