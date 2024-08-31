import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { SortDirection } from '@angular/material/sort';
import { Observable, map } from 'rxjs';
import { GMBaseActDTO } from './gm-base-act.dto';
import { GMBaseDTO } from './gm-base.dto';
import { GMBase } from './gm-base.interface';

@Injectable({
  providedIn: 'root'
})
export class GMBaseService {
  private httpClient = inject(HttpClient);
  private readonly href = 'http://localhost:3000/gm_base';

  constructor() { }

  getGMs(
    sort: string,
    order: SortDirection,
    page: number,
    limit: number
  ): Observable<{ items: GMBase[], total_count: number }> {
    return this.httpClient.get<GMBase[]>(this.href, {
      params: {
        _sort: sort,
        _order: order,
        _page: page + 1,
        _limit: limit,
      }
    })
      .pipe(
        map(items => ({
          items: items,
          total_count: 5
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
