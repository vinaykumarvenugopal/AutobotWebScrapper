using System;
using System.Collections.Generic;
using System.Text;

namespace AutobotWebScrapper.Application.Common.Exceptions
{
    public class APICallException : Exception
    {
        public int StatusCode { get; set; }
        public string Content { get; set; }
    }
}
