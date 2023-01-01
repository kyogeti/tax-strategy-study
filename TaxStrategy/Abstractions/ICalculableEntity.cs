using TaxStrategy.Enumerations;
using TaxStrategy.Models;

namespace TaxStrategy.Abstractions;

public interface ICalculableEntity
{
    decimal GetTaxRate(TaxType taxType);
    decimal GetBaseAmount();
    string? GetUf();
    IEnumerable<ApplicableTax> GetApplicableTaxes();
}