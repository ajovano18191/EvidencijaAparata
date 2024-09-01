export interface GMLocation {
  id: number;
  rul_base_id: number;
  naziv: string;
  adresa: string;
  mesto: City;
  ip: string;
  act_location_id: number | undefined;
}

export interface City {
  id: number;
  naziv: string;
}
