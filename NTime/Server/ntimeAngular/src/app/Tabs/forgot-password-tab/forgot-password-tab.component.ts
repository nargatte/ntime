import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material';
import { AuthenticationService } from '../../Services/authentication.service';
// tslint:disable-next-line:max-line-length
import { SuccessfullActionDialogComponent } from '../../SharedComponents/Dialogs/successfull-action-dialog/successfull-action-dialog.component';
import { FailedActionDialogComponent } from '../../SharedComponents/Dialogs/failed-action-dialog/failed-action-dialog.component';

@Component({
  selector: 'app-forgot-password-tab',
  templateUrl: './forgot-password-tab.component.html',
  styleUrls: ['./forgot-password-tab.component.css']
})
export class ForgotPasswordTabComponent implements OnInit {
  public emailAddress = '';
  public dataLoaded = true;

  constructor(private dialog: MatDialog, private authenticationService: AuthenticationService) {}

  ngOnInit() {}

  public successModalUp() {
    this.dataLoaded = true;
    this.dialog.open(SuccessfullActionDialogComponent, {
      data: { text: 'Wiadomość z instrukcjami została wysłana na podany adres email' }
    });
  }

  public failedModalUp(message?: string) {
    this.dataLoaded = true;
    this.dialog.open(FailedActionDialogComponent, {
      data: { text: message }
    });
  }

  public ButtonClick() {
    if (!this.emailAddress || this.emailAddress === '') {
      this.failedModalUp('Wypełnij poprawnie pole z adresem mailowym');
      return;
    }

    console.log(this.emailAddress);
    this.dataLoaded = false;
    this.authenticationService.SendForgotPassword(this.emailAddress).subscribe(
      () => this.successModalUp(),
      () => this.failedModalUp(`Użytkownik o podanym adresie nie istnieje w bazie`),
      () => this.dataLoaded = true
    );

  }
}
