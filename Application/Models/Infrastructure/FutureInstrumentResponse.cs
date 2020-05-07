using System;
using System.Collections.Generic;
using System.Text;

namespace AutobotWebScrapper.Application.Models.Infrastructure
{
    public class FutureInstrumentResponse
    {
        public FutureInstrumentResponse()
        {
            Details = new List<InstrumentDetail>();
        }
        public string SymbolName { get; set; }

        public bool IsFNOSecurity { get; set; }

        public IEnumerable<InstrumentDetail> Details { get; set; }
    }
    public class InstrumentDetail
    {
        public string InstrumentType { get; set; }
        public DateTime ExpiryDate { get; set; }

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
