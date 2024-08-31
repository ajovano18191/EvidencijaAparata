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
import { Observable } from 'rxjs';
import { GMBaseActDTO } from './gm-base-act.dto';
import { GMBaseService } from './gm-base.service';

@Component({
  selector: 'app-gm-act-form',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatFormFieldModule, MatInputModule, FormsModule, MatButtonModule, MatProgressSpinnerModule, MatDatepickerModule, MatNativeDateModule,],
  templateUrl: './gm-base-act-form.component.html',
  styleUrls: ['./gm-base-act-form.component.css']
})
export class GMBaseActFormComponent {
  private dialogRef = inject(MatDialogRef);
  private gmBaseService = inject(GMBaseService);
  public isLoadingResult: boolean = false;

  public matDialogData: { id: number | undefined, naziv: string | undefined, base_id: number } = inject(MAT_DIALOG_DATA);
  public isActivation: boolean = this.matDialogData.id === undefined;
  public gmBaseActDTO: GMBaseActDTO = {
    resenje: "",
    datum: new Date(),
    base_id: 0,
  }

  ngOnInit() {
    if (!this.isActivation) {
      // this.gmLocationService.getActiveLocationNapomena(this.matDialogData.id!)
      //        .subscribe(napomena => this.gmLocationActDTO.napomena = napomena);
    }
  }

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
