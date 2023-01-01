using AutoFixture;
using FluentAssertions;
using TaxStrategy.Enumerations;
using TaxStrategy.Models;
using TaxStrategy.Utils;

namespace TaxStrategy.UnitTests.Utils;

public class DefaultTaxRateResolverTests
{
    private readonly IFixture _fixture;

    public DefaultTaxRateResolverTests()
    {
        _fixture = new Fixture();
    }
    
    [Fact]
    public void ResolveTaxRate_GivenNotExistingApplicableTax_ShouldReturnZero()
    {
        DefaultTaxRateResolver.ResolveTaxRate(new List<ApplicableTax>(), _fixture.Create<TaxType>())
            .Should().Be(decimal.Zero);
    }
    
    [Fact]
    public void ResolveTaxRate_GivenNotExistingTaxType_ShouldReturnZero()
    {
        var taxes = _fixture.Build<ApplicableTax>()
            .With(x=> x.TaxType, TaxType.ICMS)
            .With(x=> x.Rate, _fixture.Create<decimal>())
            .CreateMany(1);

        DefaultTaxRateResolver.ResolveTaxRate(taxes, TaxType.Pis)
            .Should().Be(decimal.Zero);
    }

    [Theory]
    [MemberData(nameof(TaxesTestData))]
    public void ResolveTaxRate_GivenExistingTaxes_ShouldReturnRateAsExpected(
        ApplicableTax applicableTax, decimal expectedRate)
    {
        var applicableTaxes = new List<ApplicableTax>() { applicableTax };

        DefaultTaxRateResolver.ResolveTaxRate(applicableTaxes, applicableTax.TaxType)
            .Should().Be(expectedRate);
    }

    public static IEnumerable<object[]> TaxesTestData => new[]
    {
        new object[]
        {
            new ApplicableTax(TaxType.ICMS, 0.1m), 0.1m
        },
        new object[]
        {
            new ApplicableTax(TaxType.Pis, 0.2m), 0.2m
        },
        new object[]
        {
            new ApplicableTax(TaxType.Cofins, 0.3m), 0.3m
        }
    };
}