export interface GMBase {
  id: number;
  name: string;
  serial_num: string;
  old_sticker_no: string;
  work_type: GMBaseWorkType;
}

export enum GMBaseWorkType {
  SAS = 'SAS',
  APOLLO = 'APOLLO',
  ROULETE = 'ROULETE',
  COUNTERS = 'COUNTERS',
}
