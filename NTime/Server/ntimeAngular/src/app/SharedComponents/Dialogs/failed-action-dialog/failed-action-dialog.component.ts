import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

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

}
