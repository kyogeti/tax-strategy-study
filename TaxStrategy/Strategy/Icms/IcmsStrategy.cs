using TaxStrategy.Abstractions;
using TaxStrategy.Enumerations;
using TaxStrategy.Models;

namespace TaxStrategy.Strategy.Icms;

public class IcmsStrategy : BaseStrategy, ICalculationStrategy
{
    private readonly IEnumerable<IcmsRule> _icmsRules;

    public IcmsStrategy(IEnumerable<IcmsRule> icmsRules)
    {
        _icmsRules = icmsRules;
    }

    public CalculationResult Calculate(ICalculableEntity calculableEntity)
    {
        var icmsRule = GetIcmsRule(calculableEntity);
        ValidateCalculationParameters(calculableEntity, AppliesTo());

        var calculationResultAmount = icmsRule.AppliesExtraRate
            ? calculableEntity.GetBaseAmount() * (calculableEntity.GetTaxRate(AppliesTo()) + icmsRule.ExtraRate)
            : calculableEntity.GetBaseAmount() * calculableEntity.GetTaxRate(AppliesTo());

        return new CalculationResult(AppliesTo(), calculationResultAmount);
    }

    private IcmsRule GetIcmsRule(ICalculableEntity calculableEntity)
    {
        var icmsRule = _icmsRules.FirstOrDefault(x => x.Uf == calculableEntity.GetUf());

        if (icmsRule is null) 
            throw new InvalidOperationException("There is no rule registered for this Uf.");

        return icmsRule;
    }

    public TaxType AppliesTo()
    {
        return TaxType.ICMS;
    }
}