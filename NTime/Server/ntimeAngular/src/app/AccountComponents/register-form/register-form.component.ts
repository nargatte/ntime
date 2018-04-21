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
    private dialog: MatDialog
  ) { }

  ngOnInit() {
  }

  public registerButtonClick() {
    this.authenticationService.RegisterUser(this.registerData).subscribe(
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
    this.messageService.addError('Error while registering a user');
    this.messageService.addObject(error);
    this.failedModalUp();
  }

  private successModalUp() {
    this.dialog.open(SuccessfullActionDialogComponent, {
      data: { text: 'Rejestracja przebiegła prawidłwo' }
    });
  }

  public failedModalUp() {
    this.dialog.open(FailedActionDialogComponent, {
      data: { text: 'Nastąpił błąd podczas logowania' }
    });
  }


}
