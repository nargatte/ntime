import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-user-registered-dialog',
  templateUrl: './user-registered-dialog.component.html',
  styleUrls: ['./user-registered-dialog.component.css']
})
export class UserRegisteredDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<UserRegisteredDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit() {
  }

}
