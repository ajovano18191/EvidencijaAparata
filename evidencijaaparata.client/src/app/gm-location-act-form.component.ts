import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE, MAT_NATIVE_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Observable } from 'rxjs';
import { GMLocationActDTO } from './gm-location-act.dto';
import { GMLocationService } from './gm-location.service';

@Component({
  selector: 'app-gm-location-act-form',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatFormFieldModule, MatInputModule, FormsModule, MatButtonModule, MatProgressSpinnerModule, MatDatepickerModule, MatNativeDateModule,],
  providers: [
    { provide: DateAdapter, useClass: NativeDateAdapter },
    { provide: MAT_DATE_FORMATS, useValue: MAT_NATIVE_DATE_FORMATS },
    { provide: MAT_DATE_LOCALE, useValue: 'sr' },
  ],
  templateUrl: './gm-location-act-form.component.html',
  styleUrls: ['./gm-location-act-form.component.css']
})
export class GMLocationActFormComponent implements OnInit {
  private dialogRef = inject(MatDialogRef);
  private gmLocationService = inject(GMLocationService);
  public isLoadingResult: boolean = false;

  public matDialogData: { id: number | null, naziv: string | null, location_id: number } = inject(MAT_DIALOG_DATA);
  public isActivation: boolean = this.matDialogData.id === null;
  public gmLocationActDTO: GMLocationActDTO = {
    resenje: "",
    datum: new Date(),
    napomena: "",
  }

  ngOnInit() {
    if (!this.isActivation) {
      this.gmLocationService.getActiveLocationNapomena(this.matDialogData.id!)
        .subscribe(napomena => this.gmLocationActDTO.napomena = napomena);
    }
  }

  onOkClick() {
    this.isLoadingResult = true;
    let addOrUpdateLocation$ = new Observable<void>();
    if (this.isActivation) {
      addOrUpdateLocation$ = this.activateLocation();
    }
    else {
      addOrUpdateLocation$ = this.deactivateLocation();
    }
    addOrUpdateLocation$
      .subscribe(() => {
        this.dialogRef.close(true);
      });
  }

  activateLocation() {
    return this.gmLocationService.activateLocation(this.matDialogData.location_id, this.gmLocationActDTO);
  }

  deactivateLocation() {
    return this.gmLocationService.deactivateLocation(this.matDialogData.location_id, this.gmLocationActDTO);
  }
}
