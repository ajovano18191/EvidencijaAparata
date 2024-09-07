using System.ComponentModel.DataAnnotations;

namespace EvidencijaAparata.Server.Models
{
    public class GMBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public string serial_num { get; set; } = default!;
        [Required]
        public string old_sticker_no { get; set; } = default!;
        [Required]
        public string work_type { get; set; } = default!;
        public IEnumerable<GMBaseAct> GMBaseActs { get; set; } = default!;
    }
}
