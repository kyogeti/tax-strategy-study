using TaxStrategy.Abstractions;
using TaxStrategy.Enumerations;
using TaxStrategy.Models;

namespace TaxStrategy.Strategy.Pis;

public class PisStrategy : BaseStrategy, ICalculationStrategy
{
    public CalculationResult Calculate(ICalculableEntity calculableEntity)
    {
        ValidateCalculationParameters(calculableEntity, AppliesTo());
        var calculationResultAmount = calculableEntity.GetBaseAmount() * calculableEntity.GetTaxRate(AppliesTo());

        return new CalculationResult(AppliesTo(), calculationResultAmount);
    }

    public TaxType AppliesTo()
    {
        return TaxType.Pis;
    }
}