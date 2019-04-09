import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { AuthenticationService } from '../../Services/authentication.service';
// tslint:disable-next-line: max-line-length
import { SuccessfullActionDialogComponent } from '../../SharedComponents/Dialogs/successfull-action-dialog/successfull-action-dialog.component';
import { FailedActionDialogComponent } from '../../SharedComponents/Dialogs/failed-action-dialog/failed-action-dialog.component';
import { ResetPasswordBindingModel } from '../../Models/Authentication/ResetPasswordBindingModel';
import { Params, ActivatedRoute } from '@angular/router';
import { MessageService } from '../../Services/message.service';

@Component({
  selector: 'app-new-password-tab',
  templateUrl: './new-password-tab.component.html',
  styleUrls: ['./new-password-tab.component.css']
})
export class NewPasswordTabComponent implements OnInit {
  public dataLoaded = true;
  public newPasswordData = new ResetPasswordBindingModel();
  public confirmedPassword = '';

  constructor(private dialog: MatDialog, private authenticationService: AuthenticationService,
    private activatedRoute: ActivatedRoute, private messageService: MessageService) { }

  ngOnInit() {
    this.activatedRoute.queryParams.subscribe((params: Params) => {
      const userIdName = 'userId';
      const tokenName = 'token';
      if (params[userIdName] && params[tokenName]) {
        this.newPasswordData.UserId = params[userIdName];
        this.newPasswordData.Token = params[tokenName];
      } else {
        this.failedModalUp('Link jest nieprawidłowy');
      }
    });
  }

  public successModalUp() {
    this.dataLoaded = true;
    this.dialog.open(SuccessfullActionDialogComponent, {
      data: { text: 'Nowe hasło zostało ustawione. Możesz teraz zalogować się na swoje konto' }
    });
  }

  public failedModalUp(message?: string) {
    this.dataLoaded = true;
    this.dialog.open(FailedActionDialogComponent, {
      data: { text: message }
    });
  }

  public ButtonClick() {
    if (this.newPasswordData.NewPassword !== this.confirmedPassword) {
      this.failedModalUp('Obydwie wersje hasła muszą być identyczne');
      return;
    }

    this.messageService.addLog(this.newPasswordData.Token);
    this.messageService.addLog(this.newPasswordData.UserId);
    this.dataLoaded = false;
    this.authenticationService.SendResetPassword(this.newPasswordData).subscribe(
      () => this.successModalUp(),
      () => this.failedModalUp(`Nie udało się zmienić hasła`),
      () => this.dataLoaded = true
    );

  }

}
