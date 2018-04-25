import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material';
// tslint:disable-next-line:max-line-length
import { SuccessfullActionDialogComponent } from '../../SharedComponents/Dialogs/successfull-action-dialog/successfull-action-dialog.component';
import { FailedActionDialogComponent } from '../../SharedComponents/Dialogs/failed-action-dialog/failed-action-dialog.component';
import { AuthenticationService } from '../../Services/authentication.service';
import { AuthenticatedUserService } from '../../Services/authenticated-user.service';
import { MessageService } from '../../Services/message.service';

@Component({
  selector: 'app-my-account-tab',
  templateUrl: './my-account-tab.component.html',
  styleUrls: ['./my-account-tab.component.css', '../tab-style.css'],
  entryComponents: [
    SuccessfullActionDialogComponent, FailedActionDialogComponent
  ]
})
export class MyAccountTabComponent implements OnInit, AfterViewInit {


  constructor(
    private dialog: MatDialog,
    private activatedRoute: ActivatedRoute,
    private authenticationService: AuthenticationService,
    private authenticatedUserService: AuthenticatedUserService,
    private messageService: MessageService
  ) {
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

      const isLogout = params['logout'];
      if (isLogout === 'true') {
        if (this.authenticatedUserService.IsAuthenticated) {
          this.authenticationService.logOut().subscribe(
            result => this.dialog.open(SuccessfullActionDialogComponent, {
              data: { text: 'Wylogowanie nastąpiło poprawnie' }
            }),
            error => {
              this.messageService.addError('Could not log out');
              this.messageService.addObject(error);
              this.dialog.open(FailedActionDialogComponent, {
                data: { text: `Nie udało się wylogować` }
              });
            }
          );
        } else {
          this.dialog.open(FailedActionDialogComponent, {
            data: { text: 'Tylko zalogowani użytkownicy mogą się wylogować' }
          });
        }
      }
    });


  }

  ngOnInit() {

  }

  ngAfterViewInit(): void {

  }

}
