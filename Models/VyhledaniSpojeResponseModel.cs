using Newtonsoft.Json;
using System.ComponentModel;

namespace BCSH2BDAS2.Models;

public class VyhledaniSpojeResponseModel
{
    [JsonProperty("NAZEV_ZASTAVKY")]
    [DisplayName("Název zastávky")]
    public string NazevZastavky { get; set; }

    [JsonProperty("CISLO_LINKY_SPOJE")]
    [DisplayName("Linka a spoj")]
    public string CisloLinkyASpoje { get; set; }

    [JsonProperty("CAS_PRIJEZDU")]
    [DisplayName("Čas příjezdu")]
    public string CasPrijezdu { get; set; }

    [JsonProperty("CAS_ODJEZDU")]
    [DisplayName("Čas odjezdu")]
    public string CasOdjezdu { get; set; }
}