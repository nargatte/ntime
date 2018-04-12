import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-player-added-dialog',
  templateUrl: './player-added-dialog.component.html',
  styleUrls: ['./player-added-dialog.component.css']
})
export class PlayerAddedDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<PlayerAddedDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {
  }


}
