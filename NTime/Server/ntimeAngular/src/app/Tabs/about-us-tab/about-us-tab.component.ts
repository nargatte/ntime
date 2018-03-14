import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-about-us-tab',
  templateUrl: './about-us-tab.component.html',
  styleUrls: ['./about-us-tab.component.css']
})
export class AboutUsTabComponent implements OnInit {

    model = {
        left: true,
        middle: false,
        right: false
    };

  constructor() { }

  ngOnInit() {
  };

}
