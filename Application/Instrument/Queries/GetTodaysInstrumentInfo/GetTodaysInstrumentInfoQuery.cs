using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutobotWebScrapper.Application.Instrument.Queries.GetTodaysInstrumentInfo
{
    public class GetTodaysInstrumentInfoQuery : IRequest<FutureInstrumentInfoVM>
    {
        public string SymbolName { get; set; }

        public DateTime ContractExpiryDate { get; set; }
    }

    public class GetTodaysInstrumentInfoQueryValidator : AbstractValidator<GetTodaysInstrumentInfoQuery>
    {
        public GetTodaysInstrumentInfoQueryValidator()
        {
            RuleFor(p => p.SymbolName).NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.ContractExpiryDate).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEqual(DateTime.MinValue).WithMessage("{PropertyName} is required.");
        }
    }

}
