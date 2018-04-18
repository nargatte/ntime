import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterFormComponent } from '../AccountComponents/register-form/register-form.component';
import { LogInFormComponent } from '../AccountComponents/log-in-form/log-in-form.component';
import { MaterialCustomModule } from './material-custom.module';
import { FormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CommonModule, MaterialCustomModule, FormsModule
  ],
  declarations: [
    LogInFormComponent, RegisterFormComponent
  ],
  exports: [
    LogInFormComponent, RegisterFormComponent
  ]
})
export class AuthenticationModule { }
