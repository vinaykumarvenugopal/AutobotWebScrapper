using System;
using System.Collections.Generic;
using System.Text;

namespace AutobotWebScrapper.Infrastructure.NSEDataScrapper.Models
{
    
    public class GetHistoricalFuturesByDateDto
    {
        public Datum[] data { get; set; }

        public class Datum
        {
            public string FH_INSTRUMENT { get; set; }
            public string FH_SYMBOL { get; set; }
            public string FH_EXPIRY_DT { get; set; }
            public string FH_STRIKE_PRICE { get; set; }
            public string FH_OPTION_TYPE { get; set; }
            public string FH_MARKET_TYPE { get; set; }
            public string FH_OPENING_PRICE { get; set; }
            public string FH_TRADE_HIGH_PRICE { get; set; }
            public string FH_TRADE_LOW_PRICE { get; set; }
            public string FH_CLOSING_PRICE { get; set; }
            public string FH_LAST_TRADED_PRICE { get; set; }
            public string FH_PREV_CLS { get; set; }
            public string FH_SETTLE_PRICE { get; set; }
            public string FH_TOT_TRADED_QTY { get; set; }
            public string FH_TOT_TRADED_VAL { get; set; }
            public string FH_OPEN_INT { get; set; }
            public string FH_CHANGE_IN_OI { get; set; }
            public string FH_MARKET_LOT { get; set; }
            public string FH_TIMESTAMP { get; set; }
            public DateTime TIMESTAMP { get; set; }
            public float CALCULATED_PREMIUM_VAL { get; set; }
        }
    }


    

}
