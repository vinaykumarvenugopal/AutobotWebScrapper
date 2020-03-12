using System;
using System.Collections.Generic;
using System.Text;

namespace AutobotWebScrapper.Application.Models.Infrastructure
{
    public class HistoricalFuturesResponse
    {
        public HistoricalFuturesResponse()
        {
            Data = new List<HistoricalFutureOHLC>();
        }
        public string SymbolName { get; set; }

        public int LotSize { get; set; }
        public IEnumerable<HistoricalFutureOHLC> Data { get; set; }
    }

    public class HistoricalFutureOHLC
    {
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }

        public int NoOfcontracts { get; set; }

        public double TurnOverInLacs { get; set; }

    }
}
