import { Component, inject } from '@angular/core';
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
export class GmLocationActFormComponent {
  private dialogRef = inject(MatDialogRef);
  private gmLocationService = inject(GmLocationService);
  public isLoadingResult: boolean = false;

  public matDialogData: { location_id: number, activateNotDeactivate: boolean } = inject(MAT_DIALOG_DATA);
  public gmLocationActDTO: GMLocationActDTO = {
    resenje: "",
    datum: new Date(),
    loc_id: "",
    napomena: "",
  }

  onOkClick() {
    // this.isLoadingResult = true;
    console.log(this.matDialogData, this.gmLocationActDTO);
    this.dialogRef.close(true);
  }
}
