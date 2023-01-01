using TaxStrategy.Abstractions;
using TaxStrategy.Enumerations;

namespace TaxStrategy.Strategy;

public abstract class BaseStrategy
{
    protected virtual void ValidateCalculationParameters(ICalculableEntity calculableEntity, TaxType taxType)
    {
        if (calculableEntity.GetBaseAmount() <= 0m || calculableEntity.GetTaxRate(taxType) <= 0m)
            throw new InvalidOperationException("Invalid amounts for calculation.");
    }
}