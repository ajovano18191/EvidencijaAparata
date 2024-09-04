using System.ComponentModel.DataAnnotations;

namespace EvidencijaAparata.Server.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Naziv { get; set; } = default!;
    }
}
