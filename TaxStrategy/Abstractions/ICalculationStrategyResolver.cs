using TaxStrategy.Enumerations;

namespace TaxStrategy.Abstractions;

public interface ICalculationStrategyResolver
{
    ICalculationStrategy GetStrategy(TaxType taxType);
}