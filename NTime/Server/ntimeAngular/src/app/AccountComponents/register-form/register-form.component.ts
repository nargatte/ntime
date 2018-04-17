import { Component, OnInit } from '@angular/core';
import { RegisterBindingModel } from '../../Models/RegisterBindingModel';
import { AuthenticationService } from '../../Services/authentication.service';

@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html',
  styleUrls: ['./register-form.component.css']
})
export class RegisterFormComponent implements OnInit {

  public registerData: RegisterBindingModel = new RegisterBindingModel();

  constructor(private authenticationService: AuthenticationService) { }

  ngOnInit() {
  }

  public registerButtonClick() {
    this.authenticationService.RegisterUser(this.registerData);
  }

}
