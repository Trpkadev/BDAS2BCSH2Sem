using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2BDAS2.Models;

[Table("MODELY")]
public class Model
{
    [Key]
    [Column("ID_MODEL")]
    public int IdModel { get; set; }

    [Column("NAZEV")]
    public string Nazev { get; set; }

    [Column("JE_NIZKOPODLAZNI")]
    public bool JeNizkopodlazni { get; set; }

    [Column("ID_ZNACKA")]
    public int IdZnacka { get; set; }

    [Column("ID_TYP_VOZIDLA")]
    public int IdTypVozidla { get; set; }
}