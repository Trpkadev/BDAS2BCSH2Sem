using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BCSH2BDAS2.Models
{
    [Table("LOGY")]
    public class Log
    {
        [Key]
        [JsonRequired]
        [Column("ID_LOG")]
        public int IdLog { get; set; }

        [JsonRequired]
        [Column("CAS")]
        [DisplayName("Čas")]
        public DateTime Cas { get; set; }

        [JsonRequired]
        [Column("TYP_ZMENY")]
        [DisplayName("Typ změny")]
        public string TypZmeny { get; set; } = string.Empty;

        [JsonRequired]
        [Column("TABULKA")]
        [DisplayName("Tabulka")]
        public string Tabulka { get; set; } = string.Empty;

        [JsonRequired]
        [Column("NOVE_HODNOTY")]
        [DisplayName("Nové hodnoty")]
        public string NoveHodnoty { get; set; } = string.Empty;

        [JsonRequired]
        [Column("STARE_HODNOTY")]
        [DisplayName("Staré hodnoty")]
        public string StareHodnoty { get; set; } = string.Empty;
    }
}