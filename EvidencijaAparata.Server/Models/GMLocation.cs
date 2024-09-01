namespace EvidencijaAparata.Server.Models
{
    public class GMLocation
    {
        public int Id { get; set; }
        public int rul_base_id { get; set; }
        public string Naziv { get; set; } = default!;
        public string Adresa { get; set; } = default!;
        public City Mesto { get; set; } = default!;
        public string IP { get; set; } = default!;
    }
}
