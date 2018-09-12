import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

export interface ConfirmActionData {
  text: string;
  confirmed: boolean;
}

@Component({
  selector: 'app-confirm-action-dialog',
  templateUrl: './confirm-action-dialog.component.html',
  styleUrls: ['./confirm-action-dialog.component.css']
})
export class ConfirmActionDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<ConfirmActionDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ConfirmActionData) { }

  ngOnInit() {
  }

  public onYesClicked() {
    this.dialogRef.close(true);
  }

  public onNoClicked() {
    this.dialogRef.close(false);
  }
}
