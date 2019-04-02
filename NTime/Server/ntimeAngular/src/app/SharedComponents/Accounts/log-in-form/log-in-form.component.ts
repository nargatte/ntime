import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../../Services/authentication.service';
import { LoginData } from '../../../Models/Authentication/LoginData';
import { HttpErrorResponse } from '@angular/common/http';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material';
import {SuccessfullActionDialogComponent } from '../../Dialogs/successfull-action-dialog/successfull-action-dialog.component';
import { TokenInfo } from '../../../Models/Authentication/TokenInfo';
import { MessageService } from '../../../Services/message.service';
import { FailedActionDialogComponent } from '../../Dialogs/failed-action-dialog/failed-action-dialog.component';
import { AuthenticatedUserService } from '../../../Services/authenticated-user.service';
import { AuthenticatedUser } from '../../../Models/Authentication/AuthenticatedUser';
import { RoleEnum } from '../../../Models/Enums/RoleEnum';
import { RoleViewModel } from '../../../Models/Authentication/RoleViewModel';
import { RoleHelpers } from '../../../Helpers/role-helpers';

@Component({
  selector: 'app-log-in-form',
  templateUrl: './log-in-form.component.html',
  styleUrls: ['./log-in-form.component.css', '../../../app.component.css'],
  entryComponents: [
    SuccessfullActionDialogComponent, FailedActionDialogComponent
  ]
})
export class LogInFormComponent implements OnInit {

  public loginData: LoginData = new LoginData();
  public dataLoaded = true;

  constructor(
    private authenticationService: AuthenticationService,
    private messageService: MessageService,
    private dialog: MatDialog,
    private authenticatedUserService: AuthenticatedUserService
  ) { }

  ngOnInit() {
  }

  // Something like this might be extracted to a compleetely different class
  public loginButtonClick() {
    this.dataLoaded = false;
    const body = new URLSearchParams();
    body.set('username', this.loginData.username);
    body.set('password', this.loginData.password);
    body.set('grant_type', this.loginData.grant_type);
    this.messageService.addLog('Prepared URLSearchParams');
    this.messageService.addObject(body);
    this.authenticationService.login(body).subscribe(
      loggedUser => this.onSuccessfullLogin(loggedUser),
      error => this.onFailedLogin(error),
    );
  }

  private onSuccessfullLogin(loggedUser: TokenInfo) {
    this.dataLoaded = true;
    this.messageService.addLog('Logowanie przebiegło prawidło');
    this.messageService.addObject(loggedUser);
    this.authenticatedUserService.setUser(new AuthenticatedUser(
      loggedUser.access_token, 'first', 'last', loggedUser.userName, RoleEnum.Player
    ));
    this.authenticationService.getRole().subscribe(
      roleVieModel => this.onSuccessfullRoleImport(loggedUser, roleVieModel),
      error => this.onFailedLogin(error),
    );

  }

  private onSuccessfullRoleImport(loggedUser: TokenInfo, roleViewModel: RoleViewModel) {
    this.dataLoaded = true;
    const role = RoleHelpers.ConvertStringToRoleEnum(roleViewModel.Role);
    this.authenticatedUserService.User.Role =  role;
    this.messageService.addLog(`Role downloaed ${this.authenticatedUserService.User.Role}`);
    this.messageService.addLog('Displaying authenticated user');
    this.messageService.addObject(this.authenticatedUserService.User);
    this.successModalUp();
  }

  private onFailedLogin(errorResponse: HttpErrorResponse) {
    this.dataLoaded = true;
    // this.messageService.addError('Logowanie nieprawidłowe');
    // this.messageService.addError(errorResponse.message);
    this.messageService.addObject(errorResponse);
    this.failedModalUp(errorResponse);
  }

  public successModalUp() {
    this.dataLoaded = true;
    this.dialog.open(SuccessfullActionDialogComponent, {
      data: { text: 'Logowanie zakończone sukcesem' }
    });
  }

  public failedModalUp(errorResponse: HttpErrorResponse) {
    this.dataLoaded = true;
    this.dialog.open(FailedActionDialogComponent, {
      // data: { text: `Wystąpił błąd podczas logowania: ${errorResponse}`} // TODO
      data: { text: `Wystąpił błąd podczas logowania`}
    });
  }


}
