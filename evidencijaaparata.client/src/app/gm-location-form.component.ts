import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { GmLocationService } from './gm-location.service';
import { City, GMLocation } from './gm-location.interface';
import { GMLocationDTO } from './gm-location.dto';
import { Observable, tap } from 'rxjs';

@Component({
  selector: 'app-gm-location-form',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatFormFieldModule, MatInputModule, FormsModule, MatButtonModule, MatSelectModule, MatProgressSpinnerModule,],
  templateUrl: './gm-location-form.component.html',
  styleUrls: ['./gm-location-form.component.css']
})
export class GmLocationFormComponent {
  private dialogRef = inject(MatDialogRef);
  private gmLocationService = inject(GmLocationService);
  public isLoadingResult: boolean = false;
  public citie$: Observable<City[]> = this.gmLocationService
    .getCities()
    .pipe(
      tap(() => this.isLoadingResult = false),
  );

  public gmLocation: GMLocation | undefined = inject(MAT_DIALOG_DATA);
  public isEdit = this.gmLocation !== undefined;
  public gmLocationDTO: GMLocationDTO =
    this.isEdit ?
      { ...(this.gmLocation!), mesto_id: this.gmLocation!.mesto.id } :
      {
        rul_base_id: 0,
        naziv: "",
        adresa: "",
        ip: "",
        mesto_id: 0,
      };

  onOkClick() {
    this.isLoadingResult = true;
    let addOrUpdateLocation$ = new Observable<GMLocation>();
    if (this.isEdit) {
      addOrUpdateLocation$ = this.updateLocation();
    }
    else {
      addOrUpdateLocation$ = this.addLocation();
    }
    addOrUpdateLocation$
      .subscribe(gmLocation => {
        this.dialogRef.close(true);
      });
  }

  addLocation() {
    return this.gmLocationService.addLocation(this.gmLocationDTO);
  }

  updateLocation() {
    return this.gmLocationService.updateLocation(this.gmLocation!.id, this.gmLocationDTO);
  }
}
