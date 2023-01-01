using TaxStrategy.Abstractions;
using TaxStrategy.Enumerations;
using TaxStrategy.Utils;

namespace TaxStrategy.Models;

public class Product : ICalculableEntity
{
    public string Name { get; }
    public string Code { get; }
    public decimal Price { get; }
    public IEnumerable<ApplicableTax> ApplicableTaxes { private get; set; }

    public Product(string name, string code, decimal price, IEnumerable<ApplicableTax> applicableTaxes)
    {
        Name = name;
        Code = code;
        Price = price;
        ApplicableTaxes = applicableTaxes;
    }

    public virtual decimal GetTaxRate(TaxType taxType)
    {
        return DefaultTaxRateResolver.ResolveTaxRate(ApplicableTaxes, taxType);
    }

    public virtual decimal GetBaseAmount()
    {
        return Price;
    }

    public virtual string GetUf()
    {
        return "RJ";
    }

    public virtual IEnumerable<ApplicableTax> GetApplicableTaxes()
    {
        return ApplicableTaxes;
    }
}