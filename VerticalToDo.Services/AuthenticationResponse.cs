using System;
using VerticalToDo.Abstractions.Exceptions;

namespace VerticalToDo.Services
{
    public abstract class AuthenticationResponse : BaseResponse
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Username { get; set; }
        public Guid UserId { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Token))
                throw new CustomException("AuthenticationResponse => Token parameter should not be empty!");
            else if (string.IsNullOrEmpty(Username))
                throw new CustomException("AuthenticationResponse => Username parameter should not be empty! It should be the user's email address.");
            else if (UserId == Guid.Empty)
                throw new CustomException("AuthenticationResponse => UserId should not be empty.");
        }
    }
}
