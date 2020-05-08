using System;
using System.Collections.Generic;
using System.Text;

namespace AutobotWebScrapper.Application.Common.Exceptions
{
    public class DeserializeException : Exception
    {
        public DeserializeException(string message)
            : base(message)

        {

        }
    }
}
