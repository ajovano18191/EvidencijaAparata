import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { GmLocationService } from './gm-location.service';
import { GMLocationActDTO } from './gm-location-act.dto';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE, MAT_NATIVE_DATE_FORMATS, MatNativeDateModule, NativeDateAdapter } from '@angular/material/core';
import { Observable } from 'rxjs';

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
export class GmLocationActFormComponent implements OnInit {
  private dialogRef = inject(MatDialogRef);
  private gmLocationService = inject(GmLocationService);
  public isLoadingResult: boolean = false;

  public matDialogData: { id: number | undefined, naziv: string | undefined, location_id: number } = inject(MAT_DIALOG_DATA);
  public isActivation: boolean = this.matDialogData.id === undefined;
  public gmLocationActDTO: GMLocationActDTO = {
    resenje: "",
    datum: new Date(),
    loc_id: "",
    napomena: "",
  }

  ngOnInit() {
    if (!this.isActivation) {
      this.gmLocationService.getActiveLocationNapomena(this.matDialogData.id!)
        .subscribe(napomena =>  this.gmLocationActDTO.napomena = napomena);
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
