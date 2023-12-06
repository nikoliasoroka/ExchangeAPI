using AutoMapper;
using ExchangeAPI.Modules.Identity.Application.Common.Interfaces.Services;
using ExchangeAPI.Modules.Identity.Application.Features.Convert.Queries;
using ExchangeAPI.Modules.Identity.Shared.DTOs;
using ExchangeAPI.Shared.Common.Models.Result.Implementations.Generics;
using FluentAssertions;
using Moq;

namespace ExchangeAPI.Modules.Identity.Tests.Command;

public class ConvertCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnSuccessResultWithConvertedResponse()
    {
        // Arrange
        var exchangeRatesServiceMock = new Mock<IOpenExchangeRatesService>();
        var mapperMock = new Mock<IMapper>();

        var command = new ConvertCommand
        {
            From = "EUR",
            To = "USD",
            Value = 55.45m
        };

        var expectedResult = Result<OpenExchangeResponse>.CreateSuccess(new OpenExchangeResponse { Response = 60 });

        exchangeRatesServiceMock.Setup(x => x.ConvertAsync(It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(expectedResult);
        mapperMock.Setup(x => x.Map<ConvertResponse>(It.IsAny<ConvertCommand>()))
            .Returns((ConvertCommand cmd) => new ConvertResponse
            {
                From = cmd.From,
                To = cmd.To,
                Value = cmd.Value
            });

        var handler = new ConvertCommandHandler(exchangeRatesServiceMock.Object, mapperMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<Result<ConvertResponse>>();
        result.Success.Should().BeTrue();

        var responseData = result.Data;
        responseData.Should().NotBeNull();
        responseData.Converted.Should().Be(expectedResult.Data.Response);

        // Ensure that any other properties are correctly mapped
        mapperMock.Verify(x => x.Map<ConvertResponse>(command), Times.Once);
    }

    [Fact]
    public async Task Handle_ExceptionInService_ShouldReturnFailedResultWithError()
    {
        // Arrange
        var exchangeRatesServiceMock = new Mock<IOpenExchangeRatesService>();
        var mapperMock = new Mock<IMapper>();

        var command = new ConvertCommand
        {
            From = "EUR",
            To = "USD",
            Value = 55.45m
        };

        var expectedErrorMessage = "An error occurred in the service.";

        exchangeRatesServiceMock.Setup(x => x.ConvertAsync(It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception(expectedErrorMessage));

        var handler = new ConvertCommandHandler(exchangeRatesServiceMock.Object, mapperMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<Result<ConvertResponse>>();
        result.Success.Should().BeFalse();
        result.ErrorInfo.Should().NotBeNull();
        result.ErrorInfo.Error.Should().Be(expectedErrorMessage);
        result.Data.Should().BeNull();
    }
}