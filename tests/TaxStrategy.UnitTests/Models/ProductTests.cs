using AutoFixture;
using FluentAssertions;
using TaxStrategy.Models;

namespace TaxStrategy.UnitTests.Models;

public class ProductTests
{
    private readonly IFixture _fixture;

    public ProductTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void Product_GivenConstructor_ShouldBuildAsExpected()
    {
        var expectedName = "xpto";
        var expectedCode = "xptocode";
        var expectedPrice = 2.99m;

        var product = new Product(expectedName, expectedCode, expectedPrice, 
            _fixture.CreateMany<ApplicableTax>(3));

        product.Should().Match<Product>(src => 
            src.Name == expectedName &&
            src.Code == expectedCode &&
            src.Price == expectedPrice);
    }
}