using AutobotWebScrapper.Application.Common.Exceptions;
using AutobotWebScrapper.Application.Interfaces.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutobotWebScrapper.Application.Instrument.Queries.GetTodaysInstrumentInfo
{
    public class GetTodaysInstrumentInfoQueryHandler 
        : IRequestHandler<GetTodaysInstrumentInfoQuery, FutureInstrumentInfoVM>
    {
        private readonly IFetchFuturesDataService _fetchFuturesDataService;

        public GetTodaysInstrumentInfoQueryHandler(IFetchFuturesDataService fetchFuturesDataService)
        {
            _fetchFuturesDataService = fetchFuturesDataService;
        }
        public async Task<FutureInstrumentInfoVM> Handle(GetTodaysInstrumentInfoQuery request, 
            CancellationToken cancellationToken)
        {
            
            try
            {
                var data = await _fetchFuturesDataService.GetCurrentDayFutureInstrumentDataAsync(request.SymbolName);

                if (data.SymbolName != request.SymbolName)
                    throw new NoRecordsFoundException("Given symbol name not found");

                if(data.IsFNOSecurity == false)
                    throw new NoRecordsFoundException("Given symbol doesn't belong to futures instrument");

                var instrumentDetail = data.Details.Where(d => d.ExpiryDate == request.ContractExpiryDate)
                    .Where(d => d.InstrumentType == "Index Futures" || d.InstrumentType == "Stock Futures")
                    .SingleOrDefault();

                if (instrumentDetail == null)
                    throw new NoRecordsFoundException("Data for the requested symbol not found");

                return new FutureInstrumentInfoVM
                {
                    SymbolName = data.SymbolName,
                    LotSize = instrumentDetail.LotSize,
                    PreviousClose = instrumentDetail.PreviousClose,
                    Open = instrumentDetail.Open,
                    High = instrumentDetail.High,
                    Low = instrumentDetail.Low,
                    Close = (instrumentDetail.Close == 0 )? instrumentDetail.LastTradedPrice : instrumentDetail.Close,
                    NoOfContractsTraded = instrumentDetail.NoOfContractsTraded,
                    PercentOIChange = instrumentDetail.PercentOIChange,
                    PercentPriceChange = instrumentDetail.PercentPriceChange,
                    VWAP = instrumentDetail.VWAP,
                    AnnualisedVolatility = instrumentDetail.AnnualisedVolatility,
                    DailyVolatility = instrumentDetail.DailyVolatility,
                    LastTradedPrice = instrumentDetail.LastTradedPrice
                };

            }
            catch (NoRecordsFoundException nfe)
            {
                throw new NoRecordsFoundException(nfe.Message);
               
            }
            catch(Exception e)
            {
                throw e;
            }
     
        }
    }
}
