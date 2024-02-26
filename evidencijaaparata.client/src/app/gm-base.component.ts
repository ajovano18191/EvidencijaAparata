import { HttpClient } from '@angular/common/http';
import { Component, ViewChild, AfterViewInit, inject } from '@angular/core';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule, SortDirection } from '@angular/material/sort';
import { merge, Observable, of as observableOf, of, Subject } from 'rxjs';
import { catchError, delay, map, startWith, switchMap } from 'rxjs/operators';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { DatePipe, NgIf } from '@angular/common';
import { GMBase } from './gm-base.interface';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { GmBaseService } from './gm-base.service';
import { GmBaseFormComponent } from './gm-base-form.component';

@Component({
  selector: 'app-gm-base',
  standalone: true,
  imports: [NgIf, MatProgressSpinnerModule, MatTableModule, MatSortModule, MatPaginatorModule, DatePipe, MatButtonModule, MatIconModule, MatDialogModule, MatSlideToggleModule,],
  templateUrl: './gm-base.component.html',
  styleUrls: ['./gm-base.component.css']
})
export class GmBaseComponent implements AfterViewInit {
  displayedColumns: string[] = ['id', 'name', 'serial_num', 'old_sticker_no', 'work_type', 'act_location_naziv', 'activation', 'actions',];
  gmBaseService = inject(GmBaseService);
  data: GMBase[] = [];

  resultsLength = 0;
  isLoadingResults = true;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor() { }

  private dataUpdate$ = new Subject();

  ngAfterViewInit() {
    // If the user changes the sort order, reset back to the first page.
    this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));

    merge(this.sort.sortChange, this.paginator.page, this.dataUpdate$)
      .pipe(
        startWith({}),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.gmBaseService.getGMs(
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

  public dialog: MatDialog = inject(MatDialog);

  openGMDialog(data: GMBase | undefined) {
    const dialogRef = this.dialog.open(GmBaseFormComponent, {
      data: data
    });

    dialogRef.afterClosed()
      .subscribe(() => this.dataUpdate$.next({}));
  }

  deleteGM(id: number) {
    this.gmBaseService.deleteGM(id)
      .subscribe(() => this.dataUpdate$.next({}));
  }
}
