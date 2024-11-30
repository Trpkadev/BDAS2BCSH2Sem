using System.ComponentModel;

namespace BCSH2BDAS2.Models;

public class NakladyNaVozidlo
{
    [DisplayName("Značka")]
    public string Znacka { get; set; }

    [DisplayName("Model")]
    public string Model { get; set; }

    [DisplayName("Náklady na ujetý km")]
    public double? Naklady { get; set; }

    [DisplayName("Počet vozidel")]
    public int Pocet { get; set; }
}