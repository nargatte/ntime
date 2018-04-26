import { Component, OnInit } from '@angular/core';
import { AuthenticatedUserService } from '../../Services/authenticated-user.service';
import { AuthenticatedUser } from '../../Models/Authentication/AuthenticatedUser';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  public User: AuthenticatedUser;

  constructor(public authenticatedUserService: AuthenticatedUserService) {
    this.User = authenticatedUserService.User;
   }

  ngOnInit() {

  }



}
