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
  displayedColumns: string[] = ['id', 'name', 'serial_num', 'old_sticker_no', 'work_type',];
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

/** An example database that the data source uses to retrieve data for the table. */
export class ExampleHttpDatabase {
  constructor(private _httpClient: HttpClient) { }

  getRepoIssues(sort: string, order: SortDirection, page: number): Observable<{ items: GMBase[], total_count: number }> {
    return of({
      items: [
        {
          id: 1,
          name: 'Objekat 1',
          serial_num: 'SN001',
          old_sticker_no: 'OSN001',
          work_type: GMBaseWorkType.SAS
        },
        {
          id: 2,
          name: 'Objekat 2',
          serial_num: 'SN002',
          old_sticker_no: 'OSN002',
          work_type: GMBaseWorkType.APOLLO
        },
        {
          id: 3,
          name: 'Objekat 3',
          serial_num: 'SN003',
          old_sticker_no: 'OSN003',
          work_type: GMBaseWorkType.ROULETE
        },
        {
          id: 4,
          name: 'Objekat 4',
          serial_num: 'SN004',
          old_sticker_no: 'OSN004',
          work_type: GMBaseWorkType.COUNTERS
        },
        {
          id: 5,
          name: 'Objekat 5',
          serial_num: 'SN005',
          old_sticker_no: 'OSN005',
          work_type: GMBaseWorkType.SAS
        },
      ],
      total_count: 5,
    }).pipe(
      delay(2000),
    );
    //const href = 'https://api.github.com/search/issues';
    //const requestUrl = `${href}?q=repo:angular/components&sort=${sort}&order=${order}&page=${page + 1
    //  }`;

    //return this._httpClient.get<GithubApi>(requestUrl);
  }
}
