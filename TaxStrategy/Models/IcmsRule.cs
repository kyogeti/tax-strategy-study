namespace TaxStrategy.Models;

public class IcmsRule
{
    public bool AppliesExtraRate { get; set; }
    public string Uf { get; set; }
    public decimal ExtraRate { get; set; }
}