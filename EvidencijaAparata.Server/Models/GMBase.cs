using EvidencijaAparata.Server.DTOs;
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

        public void DTO2GMBase(GMBaseDTO gmBaseDTO)
        {
            Name = gmBaseDTO.name;
            serial_num = gmBaseDTO.serial_num;
            old_sticker_no = gmBaseDTO.old_sticker_no;
            work_type = gmBaseDTO.work_type;
        }

        public int? GetBaseActId()
        {
            return GMBaseActs?.Where(p => p.DatumDeakt == null).SingleOrDefault()?.Id;
        }
    }
}
