import { AfterViewInit, Component, ViewChild, inject } from '@angular/core';
import { CommonModule, DatePipe, NgIf } from '@angular/common';
import { GMLocation } from './gm-location.interface';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { Subject, catchError, merge, startWith, switchMap, of as observableOf, map, filter, tap } from 'rxjs';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { GmLocationService } from './gm-location.service';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { GmLocationFormComponent } from './gm-location-form.component';
import { GmLocationActFormComponent } from './gm-location-act-form.component';

@Component({
  selector: 'app-gm-location',
  standalone: true,
  imports: [NgIf, MatProgressSpinnerModule, MatTableModule, MatSortModule, MatPaginatorModule, DatePipe, MatButtonModule, MatIconModule, MatDialogModule, MatSlideToggleModule,],
  templateUrl: './gm-location.component.html',
  styleUrls: ['./gm-location.component.css']
})
export class GmLocationComponent implements AfterViewInit {
  displayedColumns: string[] = ['id', 'rul_base_id', 'naziv', 'adresa', 'mesto_naziv', 'ip', 'act_location_naziv', 'activation', 'actions',];
  gmLocationService = inject(GmLocationService);
  data: GMLocation[] = [];

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
          return this.gmLocationService.getLocations(
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

  openLocationDialog(data: GMLocation | undefined) {
    const dialogRef = this.dialog.open(GmLocationFormComponent, {
      data: data
    });

    dialogRef.afterClosed()
      .pipe(
        filter((p: boolean) => p),
      )
      .subscribe(() => this.dataUpdate$.next({}));
  }

  deleteLocation(id: number) {
    this.isLoadingResults = true;
    this.gmLocationService.deleteLocation(id)
      .subscribe(() => this.dataUpdate$.next({}));
  }

  activateOrDeactivateLocation(event: { checked: boolean, source: any }, gmLocation: GMLocation) {  
    event.source._checked = !event.source._checked;
    const dialogRef = this.dialog.open(GmLocationActFormComponent, {
      data: {
        id: gmLocation.act_location_id,
        naziv: gmLocation.act_location_naziv ? gmLocation.act_location_naziv : gmLocation.naziv,
        location_id: gmLocation.id,
      }
    });

    dialogRef.afterClosed()
      .pipe(
        filter((p: boolean) => p),
      )
      .subscribe(() => {
        this.dataUpdate$.next({});
      });
  }
}
