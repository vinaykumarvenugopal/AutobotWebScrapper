using AutobotWebScrapper.Application.ATP.Queries.GetATPInfoBySymbol;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Application.UnitTests
{
    public class GetATPBySymbolQueryValidatorShould
    {
        [Fact]
        public void ReturnValidWhenValidInputPassed()
        {
            //Arrange
            var validator = new GetATPBySymbolQueryValidator();
            //Act
            var validationResult = validator.Validate(new GetATPBySymbolQuery
            {
                SymbolName = "INFY",
                PreviousContractFromDate = new DateTime(2018, 6, 29),
                PreviousContractToDate = new DateTime(2018, 7, 26),
                PreviousContractExpiryDate = new DateTime(2018, 7, 26),
                FirstDayOfNewContract = new DateTime(2018, 7, 27),
                NewContractExpiryDate = new DateTime(2018, 8, 29)
            });

            //Assert
            validationResult.IsValid.Should().BeTrue("Validation Passed");
        }
        [Theory]
        [MemberData(nameof(InvalidInput), MemberType = typeof(GetATPBySymbolQueryValidatorShould))]
        public void ReturnInValidWhenInvalidInputPassed(GetATPBySymbolQuery input)
        {
            //Arrange
            var validator = new GetATPBySymbolQueryValidator();
            //Act
            var validationResult = validator.Validate(input);
            //Assert
            validationResult.IsValid.Should().BeFalse("Validation failed");

        }


        public static IEnumerable<object[]> InvalidInput =>
         new List<object[]>
         {
            new object[] { new GetATPBySymbolQuery () },
            new object[] { new GetATPBySymbolQuery { SymbolName = "INFY"} },
            new object[] { new GetATPBySymbolQuery { SymbolName = "INFY",
                PreviousContractFromDate = new DateTime(2018, 3, 1), PreviousContractToDate = new DateTime(2018, 3, 10)} },
            new object[] { new GetATPBySymbolQuery { SymbolName = "INFY",
                PreviousContractFromDate = new DateTime(2018, 3, 1), PreviousContractToDate = new DateTime(2018, 3, 10),
                PreviousContractExpiryDate = new DateTime(2018, 3, 10),  FirstDayOfNewContract = new DateTime(2018, 3, 11)}},
            new object[] { new GetATPBySymbolQuery { SymbolName = "INFY",
                PreviousContractFromDate = new DateTime(2018, 7, 26), PreviousContractToDate = new DateTime(2018, 6, 29),
                PreviousContractExpiryDate = new DateTime(2018, 7, 26),  FirstDayOfNewContract = new DateTime(2018, 7, 27),
                NewContractExpiryDate = new DateTime(2018, 8, 29)}},
            new object[] { new GetATPBySymbolQuery { SymbolName = "INFY",
                PreviousContractFromDate = new DateTime(2018, 6, 29), PreviousContractToDate = new DateTime(2018, 7, 26),
                PreviousContractExpiryDate = new DateTime(2018, 7, 27),  FirstDayOfNewContract = new DateTime(2018, 7, 27),
                NewContractExpiryDate = new DateTime(2018, 8, 29)}},
             new object[] { new GetATPBySymbolQuery { SymbolName = "INFY",
                PreviousContractFromDate = new DateTime(2018, 6, 29), PreviousContractToDate = new DateTime(2018, 7, 26),
                PreviousContractExpiryDate = new DateTime(2018, 7, 26),  FirstDayOfNewContract = new DateTime(2018, 7, 26),
                NewContractExpiryDate = new DateTime(2018, 8, 29)}},
             new object[] { new GetATPBySymbolQuery { SymbolName = "INFY",
                PreviousContractFromDate = new DateTime(2018, 6, 29), PreviousContractToDate = new DateTime(2018, 7, 26),
                PreviousContractExpiryDate = new DateTime(2018, 7, 26),  FirstDayOfNewContract = new DateTime(2018, 7, 27),
                NewContractExpiryDate = new DateTime(2018, 7, 27)}}
         };
    }
}
