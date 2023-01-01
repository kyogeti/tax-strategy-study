using FluentAssertions;
using Moq;
using TaxStrategy.Abstractions;
using TaxStrategy.Enumerations;
using TaxStrategy.Models;
using TaxStrategy.Strategy.Icms;

namespace TaxStrategy.UnitTests.Strategy.Icms;

public class IcmsStrategyTests
{
    private readonly Mock<ICalculableEntity> _calculableEntityMock;
    private readonly IcmsStrategy _icmsStrategy;

    public IcmsStrategyTests()
    {
        _calculableEntityMock = new Mock<ICalculableEntity>();
        _calculableEntityMock.Setup(x => x.GetUf())
            .Returns("RJ");
        var icmsRules = new List<IcmsRule>()
        {
            new()
            {
                AppliesExtraRate = false,
                ExtraRate = 0m,
                Uf = "MG"
            },
            new()
            {
                AppliesExtraRate = true,
                ExtraRate = 0.02m,
                Uf = "RJ"
            }
        };
        _icmsStrategy = new IcmsStrategy(icmsRules);
    }

    [Fact]
    public void AppliesTo_GivenTaxTypes_ShouldReturnAsExpected()
    {
        _icmsStrategy.AppliesTo().Should().Be(TaxType.ICMS);
    }

    [Theory]
    [InlineData("SP")]
    [InlineData("GO")]
    [InlineData("DF")]
    public void Calculate_GivenNotExistingUfInIcmsRulesOfCalculableEntity_ShouldThrowInvalidOperationException(string uf)
    {
        _calculableEntityMock.Setup(x => x.GetUf())
            .Returns(uf);

        Action action = () => _icmsStrategy.Calculate(_calculableEntityMock.Object);

        action
            .Should()
            .ThrowExactly<InvalidOperationException>()
            .WithMessage("There is no rule registered for this Uf.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-0.01)]
    public void Calculate_GivenInvalidBaseAmountOfCalculableEntity_ShouldThrowInvalidOperationException(decimal baseAmount)
    {
        _calculableEntityMock.Setup(x => x.GetBaseAmount())
            .Returns(baseAmount);

        Action action = () => _icmsStrategy.Calculate(_calculableEntityMock.Object);

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
        _calculableEntityMock.Setup(x => x.GetTaxRate(TaxType.ICMS))
            .Returns(taxRate);

        Action action = () => _icmsStrategy.Calculate(_calculableEntityMock.Object);

        action
            .Should()
            .ThrowExactly<InvalidOperationException>()
            .WithMessage("Invalid amounts for calculation.");
    }

    [Theory]
    [InlineData(100, 0.1, 10, "MG", TaxType.ICMS)]
    [InlineData(100, 0.2, 22, "RJ", TaxType.ICMS)]
    [InlineData(100, 0.01, 3, "RJ", TaxType.ICMS)]
    [InlineData(1234.56, 0.2, 271.6032, "RJ", TaxType.ICMS)]
    [InlineData(1234.56, 0.2, 246.912, "MG", TaxType.ICMS)]
    public void Calculate_GivenValidCalculationParameters_ShouldReturnAsExpected(
        decimal baseAmount, decimal taxRate, decimal expectedAmountResult, string uf, TaxType expectedTaxTypeResult)
    {
        _calculableEntityMock.Setup(x => x.GetTaxRate(TaxType.ICMS))
            .Returns(taxRate);
        _calculableEntityMock.Setup(x => x.GetBaseAmount())
            .Returns(baseAmount);
        _calculableEntityMock.Setup(x => x.GetUf())
            .Returns(uf);

        var result = _icmsStrategy.Calculate(_calculableEntityMock.Object);

        result
            .Should()
            .Match<CalculationResult>(src => 
                src.TaxAmount == expectedAmountResult &&
                src.TaxType == expectedTaxTypeResult);
    }
}