using EvidencijaAparata.Server.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvidencijaAparata.Server.Models
{
    public class GMLocation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int rul_base_id { get; set; }
        [Required] 
        public string Naziv { get; set; } = default!;
        [Required]
        public string Adresa { get; set; } = default!;
        [Required]
        public City Mesto { get; set; } = default!;
        [Required]
        // [RegularExpression(@"^(?:(?:25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.){3}(?:25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])$")]
        public string IP { get; set; } = default!;
        public IEnumerable<GMLocationAct> GMLocationActs { get; set; }

        public GMLocation() { }

        public void DTO2GMLocation(GMLocationDTO gmLocationDTO, City city)
        {
            rul_base_id = gmLocationDTO.rul_base_id;
            Naziv = gmLocationDTO.naziv;
            Adresa = gmLocationDTO.adresa;
            Mesto = city;
            IP = gmLocationDTO.IP;
        }

        public int? GetLocationActId()
        {
            return GMLocationActs?.Where(p => p.DatumDeakt == null).SingleOrDefault()?.Id;
        }
    }
}
