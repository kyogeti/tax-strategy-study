using FluentAssertions;
using TaxStrategy.Abstractions;
using TaxStrategy.Enumerations;
using TaxStrategy.Models;
using TaxStrategy.Strategy.Cofins;
using TaxStrategy.Strategy.Icms;
using TaxStrategy.Strategy.Pis;

namespace TaxStrategy.UnitTests.Strategy;

public class StrategyResolversTests
{
    [Theory]
    [MemberData(nameof(NotExistingStrategyTestData))]
    public void GetStrategy_GivenNotExistingStrategyForType_ShouldThrowInvalidOperationException(
        ICalculationStrategyResolver calculationStrategyResolver, TaxType taxType)
    {
        Action action = () => calculationStrategyResolver.GetStrategy(taxType);
        
        action
            .Should()
            .ThrowExactly<InvalidOperationException>()
            .WithMessage($"There is not strategy registered for {taxType}");
    }
    
    [Theory]
    [MemberData(nameof(ExistingStrategyTestData))]
    public void GetStrategy_GivenExistingStrategyForType_ShouldReturnStrategyAsExpected(
        ICalculationStrategyResolver calculationStrategyResolver, TaxType taxType, Type expectedType)
    {
        var result = calculationStrategyResolver.GetStrategy(taxType);

        result.Should().BeOfType(expectedType);
    }

    public static IEnumerable<object[]> NotExistingStrategyTestData = new[]
    {
        new object[]
        {
            new PisStrategyResolver(GetEmptyStrategiesList()), TaxType.Pis
        },
        new object[]
        {
            new CofinsStrategyResolver(GetEmptyStrategiesList()), TaxType.Cofins
        },
        new object[]
        {
            new IcmsStrategyResolver(GetEmptyStrategiesList()), TaxType.ICMS
        },
    };
    
    public static IEnumerable<object[]> ExistingStrategyTestData = new[]
    {
        new object[]
        {
            new PisStrategyResolver(GetStrategiesList()), TaxType.Pis, typeof(PisStrategy)
        },
        new object[]
        {
            new CofinsStrategyResolver(GetStrategiesList()), TaxType.Cofins, typeof(CofinsStrategy)
        },
        new object[]
        {
            new IcmsStrategyResolver(GetStrategiesList()), TaxType.ICMS, typeof(IcmsStrategy)
        },
    };

    private static IEnumerable<ICalculationStrategy> GetEmptyStrategiesList() => new List<ICalculationStrategy>();
    private static IEnumerable<ICalculationStrategy> GetStrategiesList() => new List<ICalculationStrategy>()
    {
        new PisStrategy(), new CofinsStrategy(), new IcmsStrategy(new List<IcmsRule>
        {
            new()
        })
    };
}