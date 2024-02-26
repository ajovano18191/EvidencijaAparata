import { GMBaseWorkType } from "./gm-base.interface";

export interface GMBaseDTO {
  name: string;
  serial_num: string;
  old_sticker_no: string;
  work_type: GMBaseWorkType;
}
