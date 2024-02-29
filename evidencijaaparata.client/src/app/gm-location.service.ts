import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { SortDirection } from '@angular/material/sort';
import { GMLocation } from './gm-location.interface';
import { Observable, map } from 'rxjs';
import { GMLocationDTO } from './gm-location.dto';

@Injectable({
  providedIn: 'root'
})
export class GmLocationService {
  private httpClient = inject(HttpClient);
  private readonly href = 'http://localhost:3000/gm_location';

  constructor() { }

  getLocations(
    sort: string,
    order: SortDirection,
    page: number,
    limit: number
  ): Observable<{ items: GMLocation[], total_count: number }> {
    return this.httpClient.get<GMLocation[]>(this.href, {
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

  addLocation(gmDTO: GMLocationDTO): Observable<GMLocation> {
    return this.httpClient.post<GMLocation>(this.href, gmDTO);
  }

  updateLocation(id: number, gmDTO: GMLocationDTO): Observable<GMLocation> {
    return this.httpClient.put<GMLocation>(`${this.href}/${id}`, gmDTO);
  }

  deleteLocation(id: number): Observable<void> {
    return this.httpClient.delete<void>(`${this.href}/${id}`);
  }
}
