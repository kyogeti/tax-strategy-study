using TaxStrategy.Enumerations;

namespace TaxStrategy.Models;

public record ApplicableTax(TaxType TaxType, decimal Rate);