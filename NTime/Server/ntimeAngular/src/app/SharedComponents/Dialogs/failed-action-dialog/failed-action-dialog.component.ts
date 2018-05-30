import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-failed-action-dialog',
  templateUrl: './failed-action-dialog.component.html',
  styleUrls: ['./failed-action-dialog.component.css']
})
export class FailedActionDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<FailedActionDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {
  }

  onNoClick(): void {
    this.dialogRef.close(this.data);
  }

}
