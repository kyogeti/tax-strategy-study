using FluentAssertions;
using Moq;
using TaxStrategy.Abstractions;
using TaxStrategy.Enumerations;
using TaxStrategy.Models;
using TaxStrategy.Strategy.Cofins;

namespace TaxStrategy.UnitTests.Strategy.Cofins;

public class CofinsStrategyTests
{
    private readonly Mock<ICalculableEntity> _calculableEntityMock;
    private readonly CofinsStrategy _cofinsStrategy;

    public CofinsStrategyTests()
    {
        _calculableEntityMock = new Mock<ICalculableEntity>();
        _cofinsStrategy = new CofinsStrategy();
    }

    [Fact]
    public void AppliesTo_GivenTaxTypes_ShouldReturnAsExpected()
    {
        _cofinsStrategy.AppliesTo().Should().Be(TaxType.Cofins);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-0.01)]
    public void Calculate_GivenInvalidBaseAmountOfCalculableEntity_ShouldThrowInvalidOperationException(decimal baseAmount)
    {
        _calculableEntityMock.Setup(x => x.GetBaseAmount())
            .Returns(baseAmount);

        Action action = () => _cofinsStrategy.Calculate(_calculableEntityMock.Object);

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
        _calculableEntityMock.Setup(x => x.GetTaxRate(TaxType.Cofins))
            .Returns(taxRate);

        Action action = () => _cofinsStrategy.Calculate(_calculableEntityMock.Object);

        action
            .Should()
            .ThrowExactly<InvalidOperationException>()
            .WithMessage("Invalid amounts for calculation.");
    }

    [Theory]
    [InlineData(100, 0.1, 10, TaxType.Cofins)]
    [InlineData(100, 0.2, 20, TaxType.Cofins)]
    [InlineData(100, 0.01, 1, TaxType.Cofins)]
    [InlineData(1234.56, 0.076, 93.82656, TaxType.Cofins)]
    public void Calculate_GivenValidCalculationParameters_ShouldReturnAsExpected(
        decimal baseAmount, decimal taxRate, decimal expectedAmountResult, TaxType expectedTaxTypeResult)
    {
        _calculableEntityMock.Setup(x => x.GetTaxRate(TaxType.Cofins))
            .Returns(taxRate);
        _calculableEntityMock.Setup(x => x.GetBaseAmount())
            .Returns(baseAmount);

        var result = _cofinsStrategy.Calculate(_calculableEntityMock.Object);

        result
            .Should()
            .Match<CalculationResult>(src => 
                src.TaxAmount == expectedAmountResult &&
                src.TaxType == expectedTaxTypeResult);
    }
}