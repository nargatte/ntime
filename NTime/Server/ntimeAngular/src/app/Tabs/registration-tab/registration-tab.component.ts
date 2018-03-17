import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-registration-tab',
  templateUrl: './registration-tab.component.html',
  styleUrls: ['./registration-tab.component.css', '../tab-style.css']
})
export class RegistrationTabComponent implements OnInit {
  public competitionId: number;

  constructor(
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.competitionId = +this.route.snapshot.paramMap.get('id');
  }

}
