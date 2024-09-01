import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Observable, map, tap } from 'rxjs';
import { GMBaseActDTO } from './gm-base-act.dto';
import { GMBaseService } from './gm-base.service';
import { GMLocation } from './gm-location.interface';
import { GMLocationService } from './gm-location.service';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-gm-act-form',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatFormFieldModule, MatInputModule, FormsModule, MatButtonModule, MatProgressSpinnerModule, MatDatepickerModule, MatNativeDateModule, MatSelectModule,],
  templateUrl: './gm-base-act-form.component.html',
  styleUrls: ['./gm-base-act-form.component.css']
})
export class GMBaseActFormComponent {
  private dialogRef = inject(MatDialogRef);
  private gmBaseService = inject(GMBaseService);
  public isLoadingResult: boolean = true;

  public matDialogData: { id: number | undefined, naziv: string | undefined, base_id: number } = inject(MAT_DIALOG_DATA);
  public isActivation: boolean = this.matDialogData.id === undefined;
  public gmBaseActDTO: GMBaseActDTO = {
    resenje: "",
    datum: new Date(),
    act_location_id: undefined,
  }

  private gmLocationService = inject(GMLocationService);
  public activeLocation$: Observable<GMLocation[]> = this.gmLocationService
    .getActiveLocations()
    .pipe(
      tap(() => this.isLoadingResult = false),
    );

  onOkClick() {
    this.isLoadingResult = true;
    let addOrUpdateBase$ = new Observable<void>();
    if (this.isActivation) {
      addOrUpdateBase$ = this.activateBase();
    }
    else {
      addOrUpdateBase$ = this.deactivateBase();
    }
    addOrUpdateBase$
      .subscribe(() => {
        this.dialogRef.close(true);
      });
  }

  activateBase() {
    return this.gmBaseService.activateBase(this.matDialogData.base_id, this.gmBaseActDTO);
  }

  deactivateBase() {
    return this.gmBaseService.deactivateBase(this.matDialogData.base_id, this.gmBaseActDTO);
  }
}
