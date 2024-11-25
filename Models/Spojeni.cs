namespace BCSH2BDAS2.Models
{
    public class Spojeni
    {
        public string Od { get; set; } = string.Empty;
        public string Do { get; set; } = string.Empty;
        public TimeOnly Odjezd { get; set; } = new TimeOnly();
        public TimeOnly Prijezd { get; set; } = new TimeOnly();
        public int Linka { get; set; }

        public TimeSpan DobaCesty => Prijezd - Odjezd;
    }
}