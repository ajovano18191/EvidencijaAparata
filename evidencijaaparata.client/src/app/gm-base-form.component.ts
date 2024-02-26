import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { GMBase, GMBaseWorkType } from './gm-base.interface';
import { GMBaseDTO } from './gm-base.dto';
import { GmBaseService } from './gm-base.service';
import { Observable, delay } from 'rxjs';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-gm-base-form',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatFormFieldModule, MatInputModule, FormsModule, MatButtonModule, MatSelectModule,],
  templateUrl: './gm-base-form.component.html',
  styleUrls: ['./gm-base-form.component.css']
})
export class GmBaseFormComponent {
  private dialogRef = inject(MatDialogRef);
  private gmBaseService = inject(GmBaseService);
  public workTypes = GMBaseWorkType;
  public gmBase: GMBase | undefined = inject(MAT_DIALOG_DATA);
  public isEdit = this.gmBase !== undefined;
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
    let addOrUpdateGM$ = new Observable<GMBase>();
    if (this.isEdit) {
      addOrUpdateGM$ = this.updateGM();
    }
    else {
      addOrUpdateGM$ = this.addGM();
    }
    addOrUpdateGM$
    .subscribe(gmBase => {
      this.gmBase = gmBase;
      this.dialogRef.close();
    });
  }

  addGM() {
    return this.gmBaseService.addGM(this.gmBaseDTO);
  }

  updateGM() {
    return this.gmBaseService.updateGM(this.gmBase!.id, this.gmBaseDTO);
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
