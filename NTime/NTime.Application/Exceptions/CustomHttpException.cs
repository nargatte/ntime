using System;
using System.Collections.Generic;
using System.Text;

namespace NTime.Application.Exceptions
{
    public class CustomHttpException : Exception
    {
        public CustomHttpException()
        {
        }

        public CustomHttpException(string message) : base(message)
        {
        }
    }
}
