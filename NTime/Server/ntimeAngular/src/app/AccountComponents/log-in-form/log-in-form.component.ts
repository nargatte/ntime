import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../Services/authentication.service';
import { LoginData } from '../../Models/LoginData';

@Component({
  selector: 'app-log-in-form',
  templateUrl: './log-in-form.component.html',
  styleUrls: ['./log-in-form.component.css']
})
export class LogInFormComponent implements OnInit {

  public loginData: LoginData = new LoginData();

  constructor(private authenticationService: AuthenticationService) { }

  ngOnInit() {
  }

  public loginButtonClick() {
    this.authenticationService.Login(this.loginData);
  }

}
