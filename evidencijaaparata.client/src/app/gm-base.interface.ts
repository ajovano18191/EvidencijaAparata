export interface GMBase {
  id: number;
  name: string;
  serial_num: string;
  old_sticker_no: string;
  work_type: GMBaseWorkType;
  act_base_id: number | undefined;
  act_location_id: number | undefined;
  act_location_naziv: number | undefined;
}

export enum GMBaseWorkType {
  SAS = 'SAS',
  APOLLO = 'APOLLO',
  ROULETE = 'ROULETE',
  COUNTERS = 'COUNTERS',
}
