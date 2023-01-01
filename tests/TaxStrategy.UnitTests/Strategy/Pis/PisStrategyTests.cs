using FluentAssertions;
using Moq;
using TaxStrategy.Abstractions;
using TaxStrategy.Enumerations;
using TaxStrategy.Models;
using TaxStrategy.Strategy.Pis;

namespace TaxStrategy.UnitTests.Strategy.Pis;

public class PisStrategyTests
{
    private readonly Mock<ICalculableEntity> _calculableEntityMock;
    private readonly PisStrategy _pisStrategy;

    public PisStrategyTests()
    {
        _calculableEntityMock = new Mock<ICalculableEntity>();
        _pisStrategy = new PisStrategy();
    }

    [Fact]
    public void AppliesTo_GivenTaxTypes_ShouldReturnAsExpected()
    {
        _pisStrategy.AppliesTo().Should().Be(TaxType.Pis);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-0.01)]
    public void Calculate_GivenInvalidBaseAmountOfCalculableEntity_ShouldThrowInvalidOperationException(decimal baseAmount)
    {
        _calculableEntityMock.Setup(x => x.GetBaseAmount())
            .Returns(baseAmount);

        Action action = () => _pisStrategy.Calculate(_calculableEntityMock.Object);

        action
            .Should()
            .ThrowExactly<InvalidOperationException>()
            .WithMessage("Invalid amounts for calculation.");
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-0.01)]
    public void Calculate_GivenInvalidTaxRateOfCalculableEntity_ShouldThrowInvalidOperationException(decimal taxRate)
    {
        _calculableEntityMock.Setup(x => x.GetTaxRate(TaxType.Pis))
            .Returns(taxRate);

        Action action = () => _pisStrategy.Calculate(_calculableEntityMock.Object);

        action
            .Should()
            .ThrowExactly<InvalidOperationException>()
            .WithMessage("Invalid amounts for calculation.");
    }

    [Theory]
    [InlineData(100, 0.1, 10, TaxType.Pis)]
    [InlineData(100, 0.2, 20, TaxType.Pis)]
    [InlineData(100, 0.01, 1, TaxType.Pis)]
    [InlineData(1234.56, 0.0165, 20.37024, TaxType.Pis)]
    public void Calculate_GivenValidCalculationParameters_ShouldReturnAsExpected(
        decimal baseAmount, decimal taxRate, decimal expectedAmountResult, TaxType expectedTaxTypeResult)
    {
        _calculableEntityMock.Setup(x => x.GetTaxRate(TaxType.Pis))
            .Returns(taxRate);
        _calculableEntityMock.Setup(x => x.GetBaseAmount())
            .Returns(baseAmount);

        var result = _pisStrategy.Calculate(_calculableEntityMock.Object);

        result
            .Should()
            .Match<CalculationResult>(src => 
                src.TaxAmount == expectedAmountResult &&
                src.TaxType == expectedTaxTypeResult);
    }
}