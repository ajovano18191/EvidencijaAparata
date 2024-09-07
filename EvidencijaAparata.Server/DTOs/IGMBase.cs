using EvidencijaAparata.Server.Models;

namespace EvidencijaAparata.Server.DTOs
{
    public record IGMBase(int id, string name, string serial_num, string old_sticker_no, string work_type, int? act_base_id, int? act_location_id, string? act_location_naziv)
    {
        public IGMBase(GMBase gmBase) :
            this(
                gmBase.Id,
                gmBase.Name,
                gmBase.serial_num,
                gmBase.old_sticker_no,
                gmBase.work_type,
                null,
                null,
                null
    )
        { 
            GMBaseAct? gmBaseAct = gmBase.GetBaseAct();
            if (gmBaseAct != null) {
                act_base_id = gmBaseAct.Id;
                act_location_id = gmBaseAct.GMLocationAct.Id;
                act_location_naziv = gmBaseAct.GMLocationAct.GMLocation.Naziv;
            }
        }
    }
    public record GMBaseDTO(string name, string serial_num, string old_sticker_no, string work_type);
    public record GMBaseActDTO(DateTime datum, string resenje, string napomena);
}
