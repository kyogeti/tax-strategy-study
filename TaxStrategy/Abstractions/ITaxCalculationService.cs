using TaxStrategy.Models;

namespace TaxStrategy.Abstractions;

public interface ITaxCalculationService
{
    IEnumerable<CalculationResult> CalculateTaxes(Product product);
}