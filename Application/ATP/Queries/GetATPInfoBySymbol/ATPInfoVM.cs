using System;
using System.Collections.Generic;
using System.Text;

namespace AutobotWebScrapper.Application.ATP.Queries.GetATPInfoBySymbol
{
    public class ATPInfoVM
    {
        public string SymbolName { get; set; }
        public double ATP { get; set; }

        public double FirstDayClosePrice { get; set; }

        public double Difference { get; set; }

        public double UpsideStrikePrice { get; set; }

        public double DownsideStrikePrice { get; set; }

    }
}
