import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { SortDirection } from '@angular/material/sort';
import { Observable, filter, map } from 'rxjs';
import { GMBaseActDTO } from './gm-base-act.dto';
import { GMBaseDTO } from './gm-base.dto';
import { GMBase } from './gm-base.interface';

@Injectable({
  providedIn: 'root'
})
export class GMBaseService {
  private httpClient = inject(HttpClient);
  // private readonly href = 'http://localhost:3000/gm_base';
  private readonly href = '/api/gm_base';

  constructor() { }

  getGMs(
    matDialogData: { location_id: number | null, addOrNotList: boolean } | null,
    sort: string,
    order: SortDirection,
    page: number,
    limit: number
  ): Observable<{ items: GMBase[], total_count: number }> {
    const params: any = {
      _sort: sort,
      _order: order,
      _page: page + 1,
      _limit: limit
    };

    if (matDialogData !== null) {
      if (matDialogData.location_id !== null && !matDialogData.addOrNotList) {
        params.location_id = matDialogData.location_id;
      }
      params.addOrNotList = matDialogData.addOrNotList;
    }

    return this.httpClient.get(this.href, {
      params: params
    })
      .pipe(
        //map(items => {
        //  return items.filter(p => !matDialogData || !(matDialogData.addOrNotList) || !(p.hasOwnProperty('location_id')));
        //}),
        map((p: any) => ({
          items: p.items ?? p,
          total_count: p.count_items ?? 5
        })),
      );
  }

  addGM(gmDTO: GMBaseDTO): Observable<GMBase> {
    return this.httpClient.post<GMBase>(this.href, gmDTO);
  }

  updateGM(id: number, gmDTO: GMBaseDTO): Observable<GMBase> {
    return this.httpClient.put<GMBase>(`${this.href}/${id}`, gmDTO);
  }

  deleteGM(id: number): Observable<void> {
    return this.httpClient.delete<void>(`${this.href}/${id}`);
  }

  activateBase(base_id: number, gmBaseActDTO: GMBaseActDTO): Observable<void> {
    return this.httpClient.put<void>(`${this.href}/${base_id}/activate`, gmBaseActDTO);
  }

  deactivateBase(base_id: number, gmBaseActDTO: GMBaseActDTO): Observable<void> {
    return this.httpClient.put<void>(`${this.href}/${base_id}/deactivate`, gmBaseActDTO);
  }
}
