using System;

namespace WishList.Infrastructure.Exceptions
{
    public sealed class LoginExistException : Exception
    {
        public string Login { get; set; }
        public LoginExistException(string login) : base($"{login} already exist")
        {
            Login = login;
            Data.Add(nameof(login), login);
        }

        public override string ToString()
        {
            var message = string.Empty;
            if (Login != null)
            {
                message = $"{Login} already exist";
            }

            return message;
        }
    }
}
