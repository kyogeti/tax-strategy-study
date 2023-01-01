using TaxStrategy.Enumerations;
using TaxStrategy.Models;

namespace TaxStrategy.Abstractions;

public interface ICalculationStrategy
{
    CalculationResult Calculate(ICalculableEntity calculableEntity);
    TaxType AppliesTo();
}