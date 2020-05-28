using System;

namespace VerticalToDo.Abstractions.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message = "You don't have sufficient permissions needed to perform this action") : base(message)
        {
        }
    }
}
