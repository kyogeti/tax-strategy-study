using TaxStrategy.Abstractions;
using TaxStrategy.Enumerations;

namespace TaxStrategy.Strategy;

public abstract class BaseStrategyResolver
{
    private readonly IEnumerable<ICalculationStrategy> _strategies;

    protected BaseStrategyResolver(IEnumerable<ICalculationStrategy> strategies)
    {
        _strategies = strategies;
    }

    protected ICalculationStrategy ResolveStrategy(TaxType taxType)
    {
        var strategy = _strategies.FirstOrDefault(x => taxType == x.AppliesTo());

        if (strategy is null)
        {
            throw new InvalidOperationException($"There is not strategy registered for {taxType}");
        }

        return strategy;
    }
}