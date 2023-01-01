using TaxStrategy.Abstractions;
using TaxStrategy.Enumerations;

namespace TaxStrategy.Strategy.Icms;

public class IcmsStrategyResolver : BaseStrategyResolver, ICalculationStrategyResolver
{
    public IcmsStrategyResolver(IEnumerable<ICalculationStrategy> strategies) : base(strategies)
    {
    }

    public ICalculationStrategy GetStrategy(TaxType taxType)
    {
        return ResolveStrategy(taxType);
    }
}