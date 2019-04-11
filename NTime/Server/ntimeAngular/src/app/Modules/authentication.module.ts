import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterFormComponent } from '../SharedComponents/Accounts/register-form/register-form.component';
import { LogInFormComponent } from '../SharedComponents/Accounts/log-in-form/log-in-form.component';
import { MaterialCustomModule } from './material-custom.module';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
// tslint:disable-next-line:max-line-length
import { PasswordRequirementsInfoComponent } from '../SharedComponents/Accounts/password-requirements-info/password-requirements-info.component';
import { ForgotPasswordTabComponent } from '../Tabs/forgot-password-tab/forgot-password-tab.component';
import { NewPasswordTabComponent } from '../Tabs/new-password-tab/new-password-tab.component';

@NgModule({
  imports: [
    CommonModule, MaterialCustomModule, FormsModule, RouterModule
  ],
  declarations: [
    LogInFormComponent, RegisterFormComponent, ForgotPasswordTabComponent,
    NewPasswordTabComponent,  PasswordRequirementsInfoComponent
  ],
  exports: [
    LogInFormComponent, RegisterFormComponent, ForgotPasswordTabComponent,
    NewPasswordTabComponent, PasswordRequirementsInfoComponent
  ]
})
export class AuthenticationModule { }
