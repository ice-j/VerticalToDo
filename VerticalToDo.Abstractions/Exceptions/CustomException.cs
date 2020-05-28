using System;

namespace VerticalToDo.Abstractions.Exceptions
{
    public class CustomException : Exception
    {
        public CustomException(string message) : base(message)
        {
        }
    }
}
