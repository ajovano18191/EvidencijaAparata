import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { Observable } from 'rxjs';
import { GMBaseDTO } from './gm-base.dto';
import { GMBase, GMBaseWorkType } from './gm-base.interface';
import { GMBaseService } from './gm-base.service';

@Component({
  selector: 'app-gm-base-form',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatFormFieldModule, MatInputModule, FormsModule, MatButtonModule, MatSelectModule, MatProgressSpinnerModule,],
  templateUrl: './gm-base-form.component.html',
  styleUrls: ['./gm-base-form.component.css']
})
export class GMBaseFormComponent {
  private dialogRef = inject(MatDialogRef);
  private gmBaseService = inject(GMBaseService);
  public workTypes = GMBaseWorkType;
  public isLoadingResult: boolean = false;
  public gmBase: GMBase | null = inject(MAT_DIALOG_DATA);
  public isEdit = this.gmBase !== null;
  public gmBaseDTO: GMBaseDTO =
    this.isEdit ?
      { ...(this.gmBase!) } :
      {
        name: "",
        serial_num: "",
        old_sticker_no: "",
        work_type: GMBaseWorkType.APOLLO,
      };

  onOkClick() {
    this.isLoadingResult = true;
    let addOrUpdateGM$ = new Observable<GMBase>();
    if (this.isEdit) {
      addOrUpdateGM$ = this.updateGM();
    }
    else {
      addOrUpdateGM$ = this.addGM();
    }
    addOrUpdateGM$
      .subscribe(gmBase => {
        this.dialogRef.close(true);
      });
  }

  addGM() {
    return this.gmBaseService.addGM(this.gmBaseDTO);
  }

  updateGM() {
    return this.gmBaseService.updateGM(this.gmBase!.id, this.gmBaseDTO);
  }
}
