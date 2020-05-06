using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutobotWebScrapper.Application.ATP.Queries.GetATPInfoBySymbol
{
    public class GetATPBySymbolQueryValidator : AbstractValidator<GetATPBySymbolQuery>
    {
        public GetATPBySymbolQueryValidator()
        {

            RuleFor(p => p.SymbolName).NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.PreviousContractFromDate).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEqual(DateTime.MinValue).WithMessage("{PropertyName} is required.");
            RuleFor(p => p.PreviousContractToDate).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEqual(DateTime.MinValue).WithMessage("{PropertyName} is required.");
            RuleFor(p => p.PreviousContractExpiryDate).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEqual(DateTime.MinValue).WithMessage("{PropertyName} is required.");
            RuleFor(p => p.FirstDayOfNewContract).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEqual(DateTime.MinValue).WithMessage("{PropertyName} is required.");
            RuleFor(p => p.NewContractExpiryDate).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().NotEqual(DateTime.MinValue).WithMessage("{PropertyName} is required.");

            RuleFor(m => new
            {
                m.PreviousContractFromDate,
                m.PreviousContractToDate,
                m.PreviousContractExpiryDate,
                m.FirstDayOfNewContract,
                m.NewContractExpiryDate
            }).Must(x => ValidContractDates(x.PreviousContractFromDate, x.PreviousContractToDate,
                    x.PreviousContractExpiryDate, x.FirstDayOfNewContract, x.NewContractExpiryDate))
                    .WithMessage("Contract dates validation failed.");
        }

        private bool ValidContractDates(DateTime PreviousContractFromDate, DateTime PreviousContractToDate,
            DateTime PreviousContractExpiryDate, DateTime FirstDayOfNewContract, DateTime NewContractExpiryDate)
        {
            if (PreviousContractToDate != PreviousContractExpiryDate) return false;
            if (PreviousContractFromDate > PreviousContractToDate) return false;
            if (FirstDayOfNewContract <= PreviousContractToDate) return false;
            if (NewContractExpiryDate <= FirstDayOfNewContract) return false;

            return true;
        }
    }
}
