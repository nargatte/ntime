import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-successfull-action-dialog',
  templateUrl: './successfull-action-dialog.component.html',
  styleUrls: ['./successfull-action-dialog.component.css']
})
export class SuccessfullActionDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<SuccessfullActionDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {
  }


}
