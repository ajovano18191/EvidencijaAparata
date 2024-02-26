import { HttpClient } from '@angular/common/http';
import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule, SortDirection } from '@angular/material/sort';
import { merge, Observable, of as observableOf, of } from 'rxjs';
import { catchError, delay, map, startWith, switchMap } from 'rxjs/operators';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CommonModule, DatePipe, NgIf } from '@angular/common';
import { GMBase, GMBaseWorkType } from './gm-base.interface';

@Component({
  selector: 'app-gm-base',
  standalone: true,
  imports: [NgIf, MatProgressSpinnerModule, MatTableModule, MatSortModule, MatPaginatorModule, DatePipe,],
  templateUrl: './gm-base.component.html',
  styleUrls: ['./gm-base.component.css']
})
export class GmBaseComponent implements AfterViewInit {
  displayedColumns: string[] = ['id', 'name', 'serial_num', 'old_sticker_no', 'work_type', 'act_location_naziv',];
  exampleDatabase: ExampleHttpDatabase | null = null;
  data: GMBase[] = [];

  resultsLength = 0;
  isLoadingResults = true;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private _httpClient: HttpClient) { }

  ngAfterViewInit() {
    this.exampleDatabase = new ExampleHttpDatabase(this._httpClient);

    // If the user changes the sort order, reset back to the first page.
    this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));

    merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        startWith({}),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.exampleDatabase!.getRepoIssues(
            this.sort.active,
            this.sort.direction,
            this.paginator.pageIndex,
            30
          ).pipe(catchError(() => observableOf(null)));
        }),
        map(data => {
          this.isLoadingResults = false;

          if (data === null) {
            return [];
          }

          this.resultsLength = data.total_count;
          return data.items;
        }),
      )
      .subscribe(data => (this.data = data));
  }
}

export class ExampleHttpDatabase {
  constructor(private _httpClient: HttpClient) { }

  getRepoIssues(sort: string, order: SortDirection, page: number, limit: number): Observable<{ items: GMBase[], total_count: number }> {
    const href = "http://localhost:3000/gm_base";
    return this._httpClient.get<GMBase[]>(href, {
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
}
