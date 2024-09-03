using EvidencijaAparata.Server.Models;

namespace EvidencijaAparata.Server.DTOs
{
    public record IGMLocation(int id, int rul_base_id, string naziv, string adresa, ICity mesto, string IP, int? act_location_id);
    public record ICity(int id, string naziv);

    //public class IGMLocation
    //{
    //    public int Id { get; set; }
    //    public int rul_base_id { get; set; }
    //    public string Naziv { get; set; } = default!;
    //    public string Adresa { get; set; } = default!;
    //    public ICity Mesto { get; set; } = default!;
    //    public string IP { get; set; } = default!;
    //    public int? act_location_id { get; set; }
    //}

    //public class ICity
    //{
    //    public int Id { get; set; }
    //    public string Naziv { get; set; } = default!;
    //}
}
