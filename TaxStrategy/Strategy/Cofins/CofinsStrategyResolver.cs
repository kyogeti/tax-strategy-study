using TaxStrategy.Abstractions;
using TaxStrategy.Enumerations;

namespace TaxStrategy.Strategy.Cofins;

public class CofinsStrategyResolver : BaseStrategyResolver, ICalculationStrategyResolver
{
    public CofinsStrategyResolver(IEnumerable<ICalculationStrategy> strategies) : base(strategies)
    {
    }

    public ICalculationStrategy GetStrategy(TaxType taxType)
    {
        return ResolveStrategy(taxType);
    }
}