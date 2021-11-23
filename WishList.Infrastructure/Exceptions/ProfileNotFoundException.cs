using System;
using System.Collections.Generic;
using System.Text;

namespace WishList.Infrastructure.Exceptions
{
    public sealed class ProfileNotFoundException : Exception
    {
        public Guid Id { get; set; }

        public ProfileNotFoundException(Guid id) : base($"{id} profile not found")
        {
            Id = id;
            Data.Add(nameof(id), id);
        }
        public override string ToString()
        {
            var message = string.Empty;
            if (Id != null)
            {
                message = $"{Id} profile not found";
            }
            return message;
        }
    }
}
