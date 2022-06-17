using System;
using System.Collections.Generic;
using System.Text;

namespace ContactsTest.Models
{
    internal class ErrorMessage
    {
        public static string Message { get; private set; }

        public ErrorMessage(string message)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }
    }
}
