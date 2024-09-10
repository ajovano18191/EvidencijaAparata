using System.ComponentModel.DataAnnotations;

namespace EvidencijaAparata.Server.Models
{
    public class GMBaseAct
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateOnly DatumAkt { get; set; }
        [Required]
        public string ResenjeAkt { get; set; } = default!;
        public DateOnly? DatumDeakt { get; set; }
        public string? ResenjeDeakt { get; set; } = default!;
        // public string sticker_no { get; set; } = default!;
        // public decimal denom_read { get; set; }
        // public decimal denom_send { get; set; }
        // public int cntIn { get; set; }
        // public int cntOut { get; set; }
        // public int cntBet { get; set; }
        // public int cntWin { get; set; }
        // public int cntGames { get; set; }
        // public int cntWinGames { get; set; }
        // public int cntBonus { get; set; }
        [Required]
        public GMBase GMBase { get; set; } = default!;
        [Required]
        public GMLocationAct GMLocationAct { get; set; } = default!;
    }
}
