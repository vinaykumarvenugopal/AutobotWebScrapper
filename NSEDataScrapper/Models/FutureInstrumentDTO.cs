using System;
using System.Collections.Generic;
using System.Text;

namespace AutobotWebScrapper.Infrastructure.NSEDataScrapper.Models
{
    public class FutureInstrumentDTO
    {
        public Info info { get; set; }
        public float underlyingValue { get; set; }
        public int vfq { get; set; }
        public string fut_timestamp { get; set; }
        public string opt_timestamp { get; set; }
        public Stock[] stocks { get; set; }
        public string[] expiryDates { get; set; }
    }


    public class Info
    {
        public string symbol { get; set; }
        public string companyName { get; set; }
        public string industry { get; set; }
        public string[] activeSeries { get; set; }
        public object[] debtSeries { get; set; }
        public object[] tempSuspendedSeries { get; set; }
        public bool isFNOSec { get; set; }
        public bool isCASec { get; set; }
        public bool isSLBSec { get; set; }
        public bool isDebtSec { get; set; }
        public bool isSuspended { get; set; }
        public bool isETFSec { get; set; }
    }

    public class Stock
    {
        public Metadata metadata { get; set; }
        public float underlyingValue { get; set; }
        public int volumeFreezeQuantity { get; set; }
        public Marketdeptorderbook marketDeptOrderBook { get; set; }
    }

    public class Metadata
    {
        public string instrumentType { get; set; }
        public string expiryDate { get; set; }
        public string optionType { get; set; }

        public string identifier { get; set; }
        public float openPrice { get; set; }
        public float highPrice { get; set; }
        public float lowPrice { get; set; }
        public float closePrice { get; set; }
        public float prevClose { get; set; }
        public float lastPrice { get; set; }
        public float change { get; set; }
        public float pChange { get; set; }
        public int numberOfContractsTraded { get; set; }
        public float totalTurnover { get; set; }
    }

    public class Marketdeptorderbook
    {
        public int totalBuyQuantity { get; set; }
        public int totalSellQuantity { get; set; }
        public Bid[] bid { get; set; }
        public Ask[] ask { get; set; }
        public Carryofcost carryOfCost { get; set; }
        public Tradeinfo tradeInfo { get; set; }
        public Otherinfo otherInfo { get; set; }
    }

    public class Carryofcost
    {
        public Price price { get; set; }
        public Carry carry { get; set; }
    }

    public class Price
    {
        public float bestBuy { get; set; }
        public float bestSell { get; set; }
        public float lastPrice { get; set; }
    }

    public class Carry
    {
        public float bestBuy { get; set; }
        public float bestSell { get; set; }
        public float lastPrice { get; set; }
    }

    public class Tradeinfo
    {
        public int tradedVolume { get; set; }
        public float value { get; set; }
        public float vmap { get; set; }
        public float premiumTurnover { get; set; }
        public float openInterest { get; set; }
        public int changeinOpenInterest { get; set; }
        public float pchangeinOpenInterest { get; set; }
        public int marketLot { get; set; }
    }

    public class Otherinfo
    {
        public float settlementPrice { get; set; }
        public float dailyvolatility { get; set; }
        public float annualisedVolatility { get; set; }
        public float impliedVolatility { get; set; }
        public int clientWisePositionLimits { get; set; }
       
    }

    public class Bid
    {
        public float price { get; set; }
        public int quantity { get; set; }
    }

    public class Ask
    {
        public float price { get; set; }
        public int quantity { get; set; }
    }


}
