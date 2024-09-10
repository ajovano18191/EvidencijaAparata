using System.ComponentModel.DataAnnotations;

namespace EvidencijaAparata.Server.Models
{
    public class GMLocationAct
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateOnly DatumAkt { get; set; }
        [Required]
        public string ResenjeAkt { get; set; } = default!;
        public DateOnly? DatumDeakt { get; set; }
        public string? ResenjeDeakt { get; set; } = default!;
        public string Napomena { get; set; } = default!;
        [Required]
        public GMLocation GMLocation { get; set; } = default!;
        public IEnumerable<GMBaseAct> GMBaseActs { get; set; } = default!;
    }
}
