﻿using AutobotWebScrapper.Application.Common.Exceptions;
using AutobotWebScrapper.Application.Interfaces.Infrastructure;
using AutobotWebScrapper.Application.Models.Infrastructure;
using AutobotWebScrapper.Common.Extensions;
using AutobotWebScrapper.Infrastructure.NSEDataScrapper.Models;

using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;


namespace AutobotWebScrapper.Infrastructure.NSEDataScrapper.Services
{
    public class FetchFuturesDataService : IFetchFuturesDataService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CancellationTokenSource _cancellationTokenSource =
            new CancellationTokenSource();
        const string TYPE = "application/json";

        public FetchFuturesDataService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ??
                       throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task<HistoricalFuturesResponse> GetHistoricalFuturesByDateAsync(string symbolName, DateTime startDate,
                    DateTime endDate, DateTime expiryDate)
        {
            var httpClient = _httpClientFactory.CreateClient("NSEClient");

            symbolName = Uri.EscapeDataString(symbolName);

            string url = UrlBuilder(symbolName, startDate, endDate, expiryDate);

            var request = new HttpRequestMessage(
                   HttpMethod.Get,
                   url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(TYPE));
            request.Headers.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36");
            request.Headers.Add("Accept-Language", "en-GB,en-US;q=0.8,en;q=0.6");


            using (var response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead,
                    _cancellationTokenSource.Token))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                if (response.IsSuccessStatusCode == false)
                {
                    throw new APICallException
                    {
                        StatusCode = (int)response.StatusCode,
                        Content = await stream.StreamToStringAsync()
                    };
                }
                GetHistoricalFuturesByDateDto returnType;
                try
                {
                    returnType =
                        await JsonSerializer.DeserializeAsync<GetHistoricalFuturesByDateDto>(stream);
                }
                catch (Exception e)
                {
                    throw new DeserializeException($"Exception occurred while deserializing " +
                        $"returned stream to FutureInstrumentDTO object. {e.Message}");
                }
                
                if (returnType.data.Count() == 0) throw new NoRecordsFoundException("No records returned from the service.");

                return new HistoricalFuturesResponse
                {
                    Data = returnType.data.Select(d => new HistoricalFutureOHLC
                    {
                        Date = StringToDate(d.FH_TIMESTAMP),
                        Open = d.FH_OPENING_PRICE.ToDouble(),
                        High = d.FH_TRADE_HIGH_PRICE.ToDouble(),
                        Low = d.FH_TRADE_LOW_PRICE.ToDouble(),
                        Close = d.FH_CLOSING_PRICE.ToDouble(),
                        NoOfcontracts = (d.FH_TOT_TRADED_QTY.ToInteger() / d.FH_MARKET_LOT.ToInteger()),
                        TurnOverInLacs = Math.Round(d.FH_TOT_TRADED_VAL.ToDouble() / 100000, 2)
                    }),
                    SymbolName = symbolName,
                    LotSize = returnType.data.FirstOrDefault().FH_MARKET_LOT.ToInteger()
                };

            }

        }
        public async Task<double> GetFuturesDayCloseByDateAsync(string symbolName, DateTime date, DateTime expiryDate)
        {
            var response = await GetHistoricalFuturesByDateAsync(symbolName, date, date, expiryDate);
            return response.Data.FirstOrDefault().Close;
        }

        public async Task<FutureInstrumentResponse> GetCurrentDayFutureInstrumentDataAsync(string symbolName)
        {
            symbolName = Uri.EscapeDataString(symbolName);

            var httpClient = _httpClientFactory.CreateClient("NSEClient");

            string url = $"api/quote-derivative?symbol={symbolName}";

            var request = new HttpRequestMessage(
                   HttpMethod.Get,
                   url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(TYPE));
            request.Headers.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36");
            request.Headers.Add("Accept-Language", "en-GB,en-US;q=0.8,en;q=0.6");

            using (var response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead,
                    _cancellationTokenSource.Token))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                if (response.IsSuccessStatusCode == false)
                {
                    throw new APICallException
                    {
                        StatusCode = (int)response.StatusCode,
                        Content = await stream.StreamToStringAsync()
                    };
                }

                FutureInstrumentDTO returnType;
                string returnedText = string.Empty;
                try
                {
                    returnType = await JsonSerializer.DeserializeAsync<FutureInstrumentDTO>(stream);
                }
                catch (Exception e)
                {
                    throw new DeserializeException($"Exception occurred while deserializing " +
                        $"returned stream to FutureInstrumentDTO object. {e.Message}");
                }

                return new FutureInstrumentResponse
                {
                    Details = returnType.stocks.Select(d => new InstrumentDetail
                    {
                        InstrumentType = d.metadata.instrumentType,
                        ExpiryDate = StringToDate(d.metadata.expiryDate),
                        Open = (double)new Decimal(d.metadata.openPrice),
                        High = (double)new Decimal(d.metadata.highPrice),
                        Low= (double)new Decimal(d.metadata.lowPrice),
                        Close = (double)new Decimal(d.metadata.closePrice),
                        PreviousClose = (double)new Decimal(d.metadata.prevClose),
                        LastTradedPrice  = (double)new Decimal(d.metadata.lastPrice),
                        PercentOIChange = (double)new Decimal(d.marketDeptOrderBook.tradeInfo.pchangeinOpenInterest),
                        PercentPriceChange = (double)new Decimal(d.metadata.pChange),
                        NoOfContractsTraded = d.metadata.numberOfContractsTraded,
                        LotSize = d.marketDeptOrderBook.tradeInfo.marketLot,
                        VWAP = (double)new Decimal(d.marketDeptOrderBook.tradeInfo.vmap),
                        AnnualisedVolatility = (double)new Decimal(d.marketDeptOrderBook.otherInfo.annualisedVolatility),
                        DailyVolatility = (double)new Decimal(d.marketDeptOrderBook.otherInfo.dailyvolatility)
                    }),
                    SymbolName = returnType.info.symbol,
                    IsFNOSecurity = returnType.info.isFNOSec
                };

            }

        }

        private string FormatDate(DateTime date) => date.ToString("dd-MM-yyyy");
        private string FormatDate2(DateTime date) => date.ToString("dd-MMM-yyyy").ToUpper();
        private string GetInstrumentType(string symbolName)
                    => (symbolName == "NIFTY" || symbolName == "BANKNIFTY") ? "FUTIDX" : "FUTSTK";

        private DateTime StringToDate(string date) => DateTime.ParseExact(date, "dd-MMM-yyyy", null);
        private string UrlBuilder(string symbolName, DateTime startDate,
                    DateTime endDate, DateTime expiryDate)
                        => new StringBuilder()
                            .Append("api/historical/fo/derivatives?")
                            .Append($"from={FormatDate(startDate)}&")
                            .Append($"to={FormatDate(endDate)}&")
                            .Append($"expiryDate={FormatDate2(expiryDate)}&")
                            .Append($"instrumentType={GetInstrumentType(symbolName)}&")
                            .Append($"symbol={symbolName}").ToString();

       
    }
}
