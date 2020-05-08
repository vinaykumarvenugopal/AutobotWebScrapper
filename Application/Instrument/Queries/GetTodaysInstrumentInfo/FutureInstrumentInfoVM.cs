using System;
using System.Collections.Generic;
using System.Text;

namespace AutobotWebScrapper.Application.Instrument.Queries.GetTodaysInstrumentInfo
{
    public class FutureInstrumentInfoVM
    {
        public string SymbolName { get; set; }

        public int LotSize { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double PreviousClose { get; set; }

        public double LastTradedPrice { get; set; }
        public double PercentPriceChange { get; set; }

        public double PercentOIChange { get; set; }

        public int NoOfContractsTraded { get; set; }

        public double VWAP { get; set; }

        public double DailyVolatility { get; set; }

        public double AnnualisedVolatility { get; set; }
    }
}
