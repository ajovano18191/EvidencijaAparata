import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { SortDirection } from '@angular/material/sort';
import { Observable, map, of } from 'rxjs';
import { GMLocationActDTO } from './gm-location-act.dto';
import { GMLocationDTO } from './gm-location.dto';
import { City, GMLocation } from './gm-location.interface';

@Injectable({
  providedIn: 'root'
})
export class GMLocationService {
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

  getActiveLocations(): Observable<GMLocation[]> {
    return this.httpClient.get<GMLocation[]>(this.href + "?act_location_id_ne");
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

  getCities(): Observable<City[]> {
    return this.httpClient.get<City[]>("http://localhost:3000/cities");
  }

  activateLocation(location_id: number, gmLocationActDTO: GMLocationActDTO): Observable<void> {
    return this.httpClient.put<void>(`${this.href}/${location_id}/activate`, gmLocationActDTO);
  }

  deactivateLocation(location_id: number, gmLocationActDTO: GMLocationActDTO): Observable<void> {
    return this.httpClient.put<void>(`${this.href}/${location_id}/deactivate`, gmLocationActDTO);
  }

  getActiveLocationNapomena(id: number): Observable<string> {
    return of("Test napomena");
  }
}
