using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutobotWebScrapper.Application.ATP.Queries.GetATPInfoBySymbol
{
    public class GetATPBySymbolQuery : IRequest<ATPInfoVM>
    {
        public string SymbolName { get; set; }

        public DateTime PreviousContractFromDate { get; set; }

        public DateTime PreviousContractToDate { get; set; }

        public DateTime PreviousContractExpiryDate { get; set; }

        public DateTime FirstDayOfNewContract { get; set; }

        public DateTime NewContractExpiryDate { get; set; }

    }
}
