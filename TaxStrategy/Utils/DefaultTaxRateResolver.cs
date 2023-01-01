using TaxStrategy.Enumerations;
using TaxStrategy.Models;

namespace TaxStrategy.Utils;

public static class DefaultTaxRateResolver
{
    public static decimal ResolveTaxRate(IEnumerable<ApplicableTax> applicableTaxes, TaxType taxType)
    {
        var applicableTax = applicableTaxes.FirstOrDefault(x => x.TaxType == taxType);
        return applicableTax?.Rate ?? 0m;
    }
}