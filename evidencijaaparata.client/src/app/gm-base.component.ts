import { DatePipe, NgIf } from '@angular/common';
import { AfterViewInit, Component, inject, InjectFlags, Optional, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { merge, of as observableOf, Subject } from 'rxjs';
import { catchError, filter, map, startWith, switchMap } from 'rxjs/operators';
import { GMBaseActFormComponent } from './gm-base-act-form.component';
import { GMBaseFormComponent } from './gm-base-form.component';
import { GMBase } from './gm-base.interface';
import { GMBaseService } from './gm-base.service';

@Component({
  selector: 'app-gm-base',
  standalone: true,
  imports: [NgIf, MatProgressSpinnerModule, MatTableModule, MatSortModule, MatPaginatorModule, DatePipe, MatButtonModule, MatIconModule, MatDialogModule, MatSlideToggleModule,],
  templateUrl: './gm-base.component.html',
  styleUrls: ['./gm-base.component.css']
})
export class GMBaseComponent implements AfterViewInit {
  displayedColumns: string[] = ['id', 'name', 'serial_num', 'old_sticker_no', 'work_type', 'act_location_naziv', 'activation', 'actions',];
  gmBaseService = inject(GMBaseService);
  data: GMBase[] = [];

  resultsLength = 0;
  isLoadingResults = true;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  public matDialogData: { act_location_id: number, addOrNotList: boolean } | null = inject(MAT_DIALOG_DATA, { optional: true });

  constructor() { }

  private dataUpdate$ = new Subject();

  ngAfterViewInit() {
    this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));

    merge(this.sort.sortChange, this.paginator.page, this.dataUpdate$)
      .pipe(
        startWith({}),
        switchMap(() => {
          this.isLoadingResults = true;
          return this.gmBaseService.getGMs(
            this.matDialogData,
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

  selectAllOrDeactivatedBases(event: { checked: boolean, source: any }) {
    if (event.source._checked) {
      this.matDialogData = null;
    }
    else {      
      this.matDialogData = {
        act_location_id: this.matDialogData?.act_location_id ?? -1,
        addOrNotList: true,
      }
    }
    this.dataUpdate$.next({});
  }

  openGMDialog(data: GMBase | undefined) {
    const dialogRef = this.dialog.open(GMBaseFormComponent, {
      data: data
    });

    dialogRef.afterClosed()
      .pipe(
        filter((p: boolean) => p),
      )
      .subscribe(() => this.dataUpdate$.next({}));
  }

  deleteGM(id: number) {
    this.isLoadingResults = true;
    this.gmBaseService.deleteGM(id)
      .subscribe(() => this.dataUpdate$.next({}));
  }

  activateOrDeactivateBase(event: { checked: boolean, source: any }, gmBase: GMBase) {
    event.source._checked = !event.source._checked;
    const dialogRef = this.dialog.open(GMBaseActFormComponent, {
      data: {
        id: gmBase.act_base_id,
        naziv: gmBase.name,
        base_id: gmBase.id,
        act_location_id: this.matDialogData?.act_location_id,
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
