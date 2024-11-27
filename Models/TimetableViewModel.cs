namespace BCSH2BDAS2.Models
{
    public class TimetableViewModel
    {
        public List<Linka> Routes { get; set; } = [];
        public int? CisloLinky { get; set; }
        public Dictionary<string, List<JizniRad>>? Timetable { get; set; }
        public int ConnectionCount => Timetable?.First().Value.Count ?? 0;
    }
}
