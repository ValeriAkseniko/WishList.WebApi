using System;
using System.Collections.Generic;
using System.Text;

namespace WishList.Infrastructure.Exceptions
{
    public sealed class ProfileNotFoundByAccountException : Exception
    {
        public Guid? AccountId { get; set; }

        public ProfileNotFoundByAccountException(Guid id) : base($"profile with account {id} not found")
        {
            AccountId = id;
            Data.Add(nameof(id), id);
        }
        public override string ToString()
        {
            var message = string.Empty;
            if (AccountId != null)
            {
                message = $"profile with account {AccountId} not found";
            }
            return message;
        }
    }
}
