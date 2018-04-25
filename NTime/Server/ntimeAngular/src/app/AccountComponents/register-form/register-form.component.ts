import { Component, OnInit } from '@angular/core';
import { RegisterBindingModel } from '../../Models/Authentication/RegisterBindingModel';
import { AuthenticationService } from '../../Services/authentication.service';
import { MessageService } from '../../Services/message.service';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material';
import { UserRegisteredDialogComponent } from '../../SharedComponents/Dialogs/user-registered-dialog/user-registered-dialog.component';
import { FailedActionDialogComponent } from '../../SharedComponents/Dialogs/failed-action-dialog/failed-action-dialog.component';
// tslint:disable-next-line:max-line-length
import { SuccessfullActionDialogComponent } from '../../SharedComponents/Dialogs/successfull-action-dialog/successfull-action-dialog.component';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Params } from '@angular/router';
import { RoleStringToEnumConverter } from '../../Helpers/RoleStringToEnumConverter';

@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html',
  styleUrls: ['./register-form.component.css'],
  entryComponents: [
    UserRegisteredDialogComponent
  ]
})
export class RegisterFormComponent implements OnInit {

  public registerData: RegisterBindingModel = new RegisterBindingModel();


  constructor(
    private authenticationService: AuthenticationService,
    private messageService: MessageService,
    private dialog: MatDialog,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit() {
  }

  public registerButtonClick() {
    let roleString = 'player';
    this.activatedRoute.queryParams.subscribe((params: Params) => {
      if (params['role']) {
        roleString = params['role'];
      }
      this.messageService.addLog(`Role string ${roleString}`);
    });
    const role = RoleStringToEnumConverter.Convert(roleString);
    this.authenticationService.registerUser(this.registerData, role).subscribe(
      registeredUser => this.onSuccessfullyRegisteredUser(registeredUser),
      error => this.onFailedRegisterUser(error)
    );
  }

  private onSuccessfullyRegisteredUser(user: RegisterBindingModel): any {
    this.messageService.addLog(`User registerd`);
    this.messageService.addObject(user);
    this.successModalUp();
  }

  private onFailedRegisterUser(error: HttpErrorResponse) {
    this.messageService.addError('Error while trying to register');
    this.messageService.addObject(error);
    this.failedModalUp();
  }

  private successModalUp() {
    this.dialog.open(SuccessfullActionDialogComponent, {
      data: { text: 'Rejestracja przebiegła prawidłowo' }
    });
  }

  public failedModalUp() {
    this.dialog.open(FailedActionDialogComponent, {
      data: { text: 'Wystąpił błąd podczas rejestracji' }
    });
  }


}
