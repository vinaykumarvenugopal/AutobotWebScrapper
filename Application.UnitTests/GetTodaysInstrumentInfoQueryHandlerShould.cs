using AutobotWebScrapper.Application.Common.Exceptions;
using AutobotWebScrapper.Application.Instrument.Queries.GetTodaysInstrumentInfo;
using AutobotWebScrapper.Application.Interfaces.Infrastructure;
using AutobotWebScrapper.Application.Models.Infrastructure;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests
{
    public class GetTodaysInstrumentInfoQueryHandlerShould
    {
        Mock<IFetchFuturesDataService> mockFuturesDataService;
        public GetTodaysInstrumentInfoQueryHandlerShould()
        {
            mockFuturesDataService = new Mock<IFetchFuturesDataService>();
        }
        [Fact]
        public async Task ReturnValidResultIfCorrectInputPassed()
        {
            //Arrange
            mockFuturesDataService
                .Setup(s => s.GetCurrentDayFutureInstrumentDataAsync(It.IsAny<string>()))
                .ReturnsAsync(new FutureInstrumentResponse
                {
                    SymbolName = "INFY",
                    IsFNOSecurity = true,
                    Details = new List<InstrumentDetail>
                        {
                            new InstrumentDetail { InstrumentType = "Stock Futures", ExpiryDate = new DateTime(2020, 5, 28)},
                            new InstrumentDetail { InstrumentType = "Stock Options", ExpiryDate = new DateTime(2020, 5, 28)}
                        }
                });
            //Act
            var sut = new GetTodaysInstrumentInfoQueryHandler(mockFuturesDataService.Object);
            var output = await sut.Handle(new GetTodaysInstrumentInfoQuery
            {
                SymbolName = "INFY",
                ContractExpiryDate = new DateTime(2020, 5, 28)
            }, new CancellationToken());

            //Assert
            using (new AssertionScope())
            {
                output.SymbolName.Should().Be("INFY");
            }
        }

        [Fact]
        public async Task ExceptionIfInvalidSymbolNamePassed()
        {
            //Arrange
            mockFuturesDataService
                .Setup(s => s.GetCurrentDayFutureInstrumentDataAsync(It.IsAny<string>()))
                .ReturnsAsync(new FutureInstrumentResponse
                {
                    SymbolName = "TATASTEEL",
                    IsFNOSecurity = true,
                    Details = new List<InstrumentDetail>
                        {
                            new InstrumentDetail { InstrumentType = "Stock Futures", ExpiryDate = new DateTime(2020, 5, 28)},
                            new InstrumentDetail { InstrumentType = "Stock Options", ExpiryDate = new DateTime(2020, 5, 28)}
                        }
                });


            //Act
            var sut = new GetTodaysInstrumentInfoQueryHandler(mockFuturesDataService.Object);
            Func<Task<FutureInstrumentInfoVM>> func = async () =>
                await sut.Handle(new GetTodaysInstrumentInfoQuery
                {
                    SymbolName = "INFY",
                    ContractExpiryDate = new DateTime(2020, 5, 28)
                }, new CancellationToken());

            //Assert
            await func.Should().ThrowAsync<NoRecordsFoundException>().WithMessage("Given symbol name not found");
        }
        [Fact]
        public async Task ExceptionIfNotFutureInstrument()
        {
            //Arrange
            mockFuturesDataService
                .Setup(s => s.GetCurrentDayFutureInstrumentDataAsync(It.IsAny<string>()))
                .ReturnsAsync(new FutureInstrumentResponse
                {
                    SymbolName = "INFY",
                    IsFNOSecurity = false,
                    Details = new List<InstrumentDetail>
                        {
                            new InstrumentDetail { InstrumentType = "Stock Futures", ExpiryDate = new DateTime(2020, 5, 28)},
                            new InstrumentDetail { InstrumentType = "Stock Options", ExpiryDate = new DateTime(2020, 5, 28)}
                        }
                });


            //Act
            var sut = new GetTodaysInstrumentInfoQueryHandler(mockFuturesDataService.Object);
            Func<Task<FutureInstrumentInfoVM>> func = async () =>
                await sut.Handle(new GetTodaysInstrumentInfoQuery
                {
                    SymbolName = "INFY",
                    ContractExpiryDate = new DateTime(2020, 5, 28)
                }, new CancellationToken());

            //Assert
            await func.Should().ThrowAsync<NoRecordsFoundException>()
                .WithMessage("Given symbol doesn't belong to futures instrument");
        }
        [Fact]
        public async Task ExceptionIfFutureContractNotPresentInCollection()
        {
            //Arrange
            mockFuturesDataService
                .Setup(s => s.GetCurrentDayFutureInstrumentDataAsync(It.IsAny<string>()))
                .ReturnsAsync(new FutureInstrumentResponse
                {
                    SymbolName = "INFY",
                    IsFNOSecurity = true,
                    Details = new List<InstrumentDetail>
                        {
                            new InstrumentDetail { InstrumentType = "Stock Index", ExpiryDate = new DateTime(2020, 5, 28)},
                            new InstrumentDetail { InstrumentType = "Stock Options", ExpiryDate = new DateTime(2020, 5, 28)}
                        }
                });


            //Act
            var sut = new GetTodaysInstrumentInfoQueryHandler(mockFuturesDataService.Object);
            Func<Task<FutureInstrumentInfoVM>> func = async () =>
                await sut.Handle(new GetTodaysInstrumentInfoQuery
                {
                    SymbolName = "INFY",
                    ContractExpiryDate = new DateTime(2020, 5, 28)
                }, new CancellationToken());

            //Assert
            await func.Should().ThrowAsync<NoRecordsFoundException>()
                .WithMessage("Data for the requested symbol not found");
        }
        [Fact]
        public async Task ExceptionIfInvalidExpiryDate()
        {
            //Arrange
            mockFuturesDataService
                .Setup(s => s.GetCurrentDayFutureInstrumentDataAsync(It.IsAny<string>()))
                .ReturnsAsync(new FutureInstrumentResponse
                {
                    SymbolName = "NIFTY",
                    IsFNOSecurity = true,
                    Details = new List<InstrumentDetail>
                        {
                            new InstrumentDetail { InstrumentType = "Index Futures", ExpiryDate = new DateTime(2020, 5, 27)},
                            new InstrumentDetail { InstrumentType = "Stock Options", ExpiryDate = new DateTime(2020, 5, 28)}
                        }
                });


            //Act
            var sut = new GetTodaysInstrumentInfoQueryHandler(mockFuturesDataService.Object);
            Func<Task<FutureInstrumentInfoVM>> func = async () =>
                await sut.Handle(new GetTodaysInstrumentInfoQuery
                {
                    SymbolName = "NIFTY",
                    ContractExpiryDate = new DateTime(2020, 5, 28)
                }, new CancellationToken());

            //Assert
            await func.Should().ThrowAsync<NoRecordsFoundException>()
                .WithMessage("Data for the requested symbol not found");
        }

        [Fact]
        public void ReturnValidWhenValidInputPassed()
        {
            //Arrange
            var validator = new GetTodaysInstrumentInfoQueryValidator();
            //Act
            var validationResult = validator.Validate(new GetTodaysInstrumentInfoQuery
            {
                SymbolName = "INFY",
                ContractExpiryDate = new DateTime(2020, 5, 28)
            });

            //Assert
            validationResult.IsValid.Should().BeTrue("Validation Passed");
        }

        [Theory]
        [MemberData(nameof(InvalidInput), MemberType = typeof(GetTodaysInstrumentInfoQueryHandlerShould))]
        public void ReturnInValidWhenInvalidInputPassed(GetTodaysInstrumentInfoQuery input)
        {
            //Arrange
            var validator = new GetTodaysInstrumentInfoQueryValidator();
            //Act
            var validationResult = validator.Validate(input);
            //Assert
            validationResult.IsValid.Should().BeFalse("Validation failed");

        }

        public static IEnumerable<object[]> InvalidInput =>
         new List<object[]>
         {
            new object[] { new GetTodaysInstrumentInfoQuery() },
            new object[] { new GetTodaysInstrumentInfoQuery { SymbolName = "INFY"} },
            new object[] { new GetTodaysInstrumentInfoQuery { SymbolName = "INFY", ContractExpiryDate = DateTime.MinValue} },
         };


    }
}
