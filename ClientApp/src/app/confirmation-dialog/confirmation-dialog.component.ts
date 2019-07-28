import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ModalData } from '../models/modal-data';

@Component({
  selector: 'confirmation-dialog',
  templateUrl: './confirmation-dialog.component.html',
  styleUrls: ['./confirmation-dialog.component.css']
})
/** confirmation-dialog component*/
export class ConfirmationDialogComponent {
  /** confirmation-dialog ctor */
  constructor(public dialogRef: MatDialogRef<ConfirmationDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: ModalData) {
  }

  onNegativeClick(): void {
    this.data.buttonNegativeHandler();
  }

  onPositiveClick(): void {
    this.data.buttonPositiveHandler();
  }
}
