import { Component } from '@angular/core';
import { UrlBuilder } from './Helpers/UrlBuilder';
import { MessageService } from './Services/message.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    title = 'Time2Win';
    constructor(private messageService: MessageService) {
        const builtString = new UrlBuilder()
                            .addControllerName('Competition')
                            .addId(15)
                            .toString();
        this.messageService.addLog(builtString);
    }
}
