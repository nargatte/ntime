import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
//import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient } from '@angular/common/http';
// import {
//     MatButtonModule, MatTabsModule, MatIconModule, MatToolbarModule
// } from '@angular/material';

//import { FlexLayoutModule } from '@angular/flex-layout';

import { AppRoutingModule } from './Modules/app-routing.module';
import {MaterialCustomModule} from './Modules/material-custom.module';

import { AppComponent } from './app.component';
import { ScoresTabComponent } from './Tabs/scores-tab/scores-tab.component';
import { MyAccountTabComponent } from './Tabs/my-account-tab/my-account-tab.component';
import { RegistrationTabComponent } from './Tabs/registration-tab/registration-tab.component';
import { OfferTabComponent } from './Tabs/offer-tab/offer-tab.component';
import { ContactTabComponent } from './Tabs/contact-tab/contact-tab.component';
import { AboutUsTabComponent } from './Tabs/about-us-tab/about-us-tab.component';
import { NavbarComponent } from './navbar/navbar.component';
import { CompetitionsSelectComponent } from './SharedComponents/competitions-select/competitions-select.component';
import { CompetitionService } from './Services/competition.service'
import { MessageService } from './Services/message.service';
//import { NewPlayerInputComponent } from './new-player-input/new-player-input.component'


@NgModule({
    imports: [
        BrowserModule, FormsModule, AppRoutingModule,
         ReactiveFormsModule, HttpClientModule, MaterialCustomModule,
        //NgbModule.forRoot(),
        // BrowserAnimationsModule, MatButtonModule, MatTabsModule, MatIconModule, //Material
        // MatToolbarModule//Material 
    ],
    declarations: [AppComponent, ScoresTabComponent, MyAccountTabComponent,
        RegistrationTabComponent, OfferTabComponent, ContactTabComponent,
        AboutUsTabComponent, NavbarComponent, CompetitionsSelectComponent],
    providers: [
        CompetitionService, MessageService
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}