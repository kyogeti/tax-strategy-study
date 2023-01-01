using TaxStrategy.Abstractions;
using TaxStrategy.Models;

namespace TaxStrategy.Services;

public class TaxCalculationService : ITaxCalculationService
{
    private readonly IEnumerable<ICalculationStrategyResolver> _strategyResolvers;

    public TaxCalculationService(IEnumerable<ICalculationStrategyResolver> strategyResolvers)
    {
        _strategyResolvers = strategyResolvers;
    }

    public IEnumerable<CalculationResult> CalculateTaxes(Product product)
    {
        return from applicableTax in product.GetApplicableTaxes()
            let strategyResolver = _strategyResolvers.FirstOrDefault(x => 
            x.GetStrategy(applicableTax.TaxType).AppliesTo() == applicableTax.TaxType) 
            where strategyResolver is not null 
            select strategyResolver.GetStrategy(applicableTax.TaxType) into strategy 
            select strategy.Calculate(product);
    }
}