using System;
using System.Collections.Generic;
using System.Text;

namespace AutobotWebScrapper.Common.Extensions
{
    public static class ConvertExtensions
    {
        public static double ToDouble( this string s)
        {
            double number;
            Double.TryParse(s, out number);
            return number;
        }
        public static int ToInteger(this string s)
        {
            int number;
            int.TryParse(s, out number);
            return number;
        }
    }
}
