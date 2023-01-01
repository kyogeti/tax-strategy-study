using AutoFixture;
using FluentAssertions;
using TaxStrategy.Abstractions;
using TaxStrategy.Enumerations;
using TaxStrategy.Models;
using TaxStrategy.Services;
using TaxStrategy.Strategy.Cofins;
using TaxStrategy.Strategy.Icms;
using TaxStrategy.Strategy.Pis;

namespace TaxStrategy.UnitTests.Services;

public class TaxCalculationServiceTests
{
    private readonly IFixture _fixture;

    public TaxCalculationServiceTests()
    {
        _fixture = new Fixture();
    }

    [Theory]
    [MemberData(nameof(ApplicableTaxesTestData))]
    public void CalculateTaxes_GivenStrategyResolvers_ShouldReturnResultsAsExpected(
        IEnumerable<ApplicableTax> applicableTaxes, int expectedResultsCount)
    {
        var service = new TaxCalculationService(BuildStrategyResolvers());

        var result = service.CalculateTaxes(BuildProduct(applicableTaxes));

        result.Should().HaveCount(expectedResultsCount);
    }

    private static IEnumerable<ICalculationStrategyResolver> BuildStrategyResolvers()
    {
        return new List<ICalculationStrategyResolver>()
        {
            new IcmsStrategyResolver(BuildStrategies()),
            new PisStrategyResolver(BuildStrategies()),
            new CofinsStrategyResolver(BuildStrategies())
        };
    }

    private static IEnumerable<ICalculationStrategy> BuildStrategies()
    {
        return new List<ICalculationStrategy>()
        {
            new IcmsStrategy(new List<IcmsRule>()
            {
                new()
                {
                    AppliesExtraRate = true,
                    ExtraRate = 0.02m,
                    Uf = "RJ"
                }
            }),
            new PisStrategy(), new CofinsStrategy()
        };
    }

    private Product BuildProduct(IEnumerable<ApplicableTax> applicableTaxes)
    {
        return new Product(
            _fixture.Create<string>(),
            _fixture.Create<string>(),
            _fixture.Create<decimal>(),
            applicableTaxes);
    }

    public static IEnumerable<object[]> ApplicableTaxesTestData => new[]
    {
        new object[]
        {
            new List<ApplicableTax>()
            {
                new(TaxType.ICMS, 0.2m),
                new(TaxType.Pis, 0.0165m),
                new(TaxType.Cofins, 0.076m)
            }, 3
        },
        new object[]
        {
            new List<ApplicableTax>()
            {
                new(TaxType.ICMS, 0.2m),
                new(TaxType.Pis, 0.0165m),
            }, 2
        },
        new object[]
        {
            new List<ApplicableTax>()
            {
                new(TaxType.ICMS, 0.2m)
            }, 1
        }
    };

} 