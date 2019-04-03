import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterFormComponent } from '../SharedComponents/Accounts/register-form/register-form.component';
import { LogInFormComponent } from '../SharedComponents/Accounts/log-in-form/log-in-form.component';
import { MaterialCustomModule } from './material-custom.module';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@NgModule({
  imports: [
    CommonModule, MaterialCustomModule, FormsModule, RouterModule
  ],
  declarations: [
    LogInFormComponent, RegisterFormComponent
  ],
  exports: [
    LogInFormComponent, RegisterFormComponent
  ]
})
export class AuthenticationModule { }
