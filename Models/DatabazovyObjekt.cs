using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2BDAS2.Models;

public class DatabazovyObjekt
{
    [Column("NAZEV")]
    [DisplayName("Název")]
    public string Nazev { get; set; } = string.Empty;

    [Column("TYP")]
    [DisplayName("Typ")]
    public string Typ { get; set; } = string.Empty;

    [Column("VYTVORENO")]
    [DisplayName("Vytvořeno")]
    public DateTime Vytvoreno { get; set; }

    [Column("POSLEDNI_PRISTUP")]
    [DisplayName("Poslední přístup")]
    public DateTime? PosledniPristup { get; set; }
}
