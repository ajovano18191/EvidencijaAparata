export interface GMLocation {
  id: number;
  rul_base_id: number;
  naziv: string;
  adresa: string;
  mesto_id: number;
  mesto_naziv: string;
  ip: string;
  act_location_naziv: string | undefined;
  act_location_id: number | undefined;
}
