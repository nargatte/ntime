import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-forgot-password-tab',
  templateUrl: './forgot-password-tab.component.html',
  styleUrls: ['./forgot-password-tab.component.css']
})
export class ForgotPasswordTabComponent implements OnInit {
  public emailAddress = '';

  constructor() { }

  ngOnInit() {
  }

}
