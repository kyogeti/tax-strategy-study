using TaxStrategy.Abstractions;
using TaxStrategy.Enumerations;

namespace TaxStrategy.Strategy.Pis;

public class PisStrategyResolver : BaseStrategyResolver, ICalculationStrategyResolver
{
    public ICalculationStrategy GetStrategy(TaxType taxType)
    {
        return ResolveStrategy(taxType);
    }

    public PisStrategyResolver(IEnumerable<ICalculationStrategy> strategies) : base(strategies)
    {
    }
}