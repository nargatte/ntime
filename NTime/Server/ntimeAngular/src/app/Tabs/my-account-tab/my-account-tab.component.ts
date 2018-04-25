import { Component, OnInit, AfterViewInit } from '@angular/core';
import {Router, ActivatedRoute, Params} from '@angular/router';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material';
// tslint:disable-next-line:max-line-length
import { SuccessfullActionDialogComponent } from '../../SharedComponents/Dialogs/successfull-action-dialog/successfull-action-dialog.component';
import { FailedActionDialogComponent } from '../../SharedComponents/Dialogs/failed-action-dialog/failed-action-dialog.component';

@Component({
  selector: 'app-my-account-tab',
  templateUrl: './my-account-tab.component.html',
  styleUrls: ['./my-account-tab.component.css', '../tab-style.css'],
  entryComponents: [
    SuccessfullActionDialogComponent, FailedActionDialogComponent
  ]
})
export class MyAccountTabComponent implements OnInit, AfterViewInit {


  constructor(private dialog: MatDialog, private activatedRoute: ActivatedRoute) {
      this.modalUp();
  }

  private modalUp() {
    this.activatedRoute.queryParams.subscribe((params: Params) => {
    const isActivated = params['isAfterActivation'];
    if (isActivated === 'true') {
      this.dialog.open(SuccessfullActionDialogComponent, {
        data: { text: 'konto zostało pomyślnie aktywowane' }
      });
    } else if (isActivated === 'false') {
      this.dialog.open(FailedActionDialogComponent, {
        data: { text: 'Konto nie zostało aktywowane' }
      });
    }
  });
  }

  ngOnInit() {

  }

  ngAfterViewInit(): void {

  }

}
