export interface GMAct {
  id: number;
  loc_act_id: number | undefined;
  loc_act_name: string | undefined;
  base_id: string | undefined;
  base_name: string | undefined;
  resenje_act: string;
  datum_act: Date;
  resenje_deact: string;
  datum_deact: Date;
  sticker_no: string;
  denom_read: number;
  denom_send: number;
  in: number;
  out: number;
  bet: number;
  win: number;
  games: number;
  win_games: number;
  bonus: number;
}
