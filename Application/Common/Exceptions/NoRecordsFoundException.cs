using System;
using System.Collections.Generic;
using System.Text;

namespace AutobotWebScrapper.Application.Common.Exceptions
{
    public class NoRecordsFoundException : Exception
    {
        public NoRecordsFoundException(string message)
           : base(message)
        {

        }
    }
}
