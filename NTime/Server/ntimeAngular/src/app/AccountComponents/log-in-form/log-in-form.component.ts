import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../Services/authentication.service';
import { LoginData } from '../../Models/Authentication/LoginData';
import { HttpErrorResponse } from '@angular/common/http';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material';
// tslint:disable-next-line:max-line-length
import {SuccessfullActionDialogComponent } from '../../SharedComponents/Dialogs/successfull-action-dialog/successfull-action-dialog.component';
import { TokenInfo } from '../../Models/Authentication/TokenInfo';
import { MessageService } from '../../Services/message.service';
import { FailedActionDialogComponent } from '../../SharedComponents/Dialogs/failed-action-dialog/failed-action-dialog.component';
import { AuthenticatedUserService } from '../../Services/authenticated-user.service';
import { AuthenticatedUser } from '../../Models/Authentication/AuthenticatedUser';
import { RoleEnum } from '../../Models/Enums/RoleEnum';
import { RoleViewModel } from '../../Models/Authentication/RoleViewModel';

@Component({
  selector: 'app-log-in-form',
  templateUrl: './log-in-form.component.html',
  styleUrls: ['./log-in-form.component.css'],
  entryComponents: [
    SuccessfullActionDialogComponent, FailedActionDialogComponent
  ]
})
export class LogInFormComponent implements OnInit {

  public loginData: LoginData = new LoginData();

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
    const body = new URLSearchParams();
    body.set('username', this.loginData.username);
    body.set('password', this.loginData.password);
    body.set('grant_type', this.loginData.grant_type);
    console.log(body);
    this.authenticationService.login(body).subscribe(
      loggedUser => this.onSuccessfullLogin(loggedUser),
      error => this.onFailedLogin(error),
    );
  }

  private onSuccessfullLogin(loggedUser: TokenInfo) {
    this.authenticationService.getRole().subscribe(
      role => this.onSuccessfullRoleImport(loggedUser, role),
      error => this.onFailedLogin(error),
    );

  }

  private onSuccessfullRoleImport(loggedUser: TokenInfo, role: RoleViewModel) {
    this.messageService.addLog('Logowanie przebiegło prawidło');
    this.messageService.addObject(loggedUser);
    this.authenticatedUserService.addUser(new AuthenticatedUser(
      loggedUser.access_token, 'first', 'last', loggedUser.userName, RoleEnum.Player
    ));
    this.authenticatedUserService.User.Role =  +role.Role;
    this.messageService.addLog(`Role downloaed ${this.authenticatedUserService.User.Role}`);
    this.messageService.addLog('Displaying authenticated user');
    this.messageService.addObject(this.authenticatedUserService.User);
    this.successModalUp();
  }

  private onFailedLogin(errorResponse: HttpErrorResponse) {
    this.messageService.addError('Logowanie nieprawidłowe');
    this.messageService.addError(errorResponse.message);
    this.messageService.addObject(errorResponse);
    this.failedModalUp();
  }

  public successModalUp() {
    this.dialog.open(SuccessfullActionDialogComponent, {
      data: { text: 'Logowanie zakończone sukcesem' }
    });
  }

  public failedModalUp() {
    this.dialog.open(FailedActionDialogComponent, {
      data: { text: 'Wystąpił błąd podczas logowania'}
    });
  }


}
