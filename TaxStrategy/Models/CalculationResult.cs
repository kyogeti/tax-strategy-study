using TaxStrategy.Enumerations;

namespace TaxStrategy.Models;

public record CalculationResult(TaxType TaxType, decimal TaxAmount);