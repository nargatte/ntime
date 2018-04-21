import { Component } from '@angular/core';
import { UrlBuilder } from './Helpers/UrlBuilder';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    title = 'Time2Win';
    constructor() {
        const builtString = new UrlBuilder()
                            .addControllerName('Competition')
                            .addId(15)
                            .toString();
        console.log(builtString);
    }
}
