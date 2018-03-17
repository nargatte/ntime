import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient } from '@angular/common/http';

// import { FlexLayoutModule } from '@angular/flex-layout';

import { AppRoutingModule } from './Modules/app-routing.module';
import { MaterialCustomModule } from './Modules/material-custom.module';

import { AppComponent } from './app.component';
import { ScoresTabComponent } from './Tabs/scores-tab/scores-tab.component';
import { MyAccountTabComponent } from './Tabs/my-account-tab/my-account-tab.component';
import { CompetitionTabComponent } from './Tabs/competition-tab/competition-tab.component';
import { OfferTabComponent } from './Tabs/offer-tab/offer-tab.component';
import { ContactTabComponent } from './Tabs/contact-tab/contact-tab.component';
import { AboutUsTabComponent } from './Tabs/about-us-tab/about-us-tab.component';
import { NavbarComponent } from './navbar/navbar.component';
import { CompetitionsSelectComponent } from './SharedComponents/competitions-select/competitions-select.component';
import { CompetitionService } from './Services/competition.service';
import { MessageService } from './Services/message.service';
import { RegistrationTabComponent } from './Tabs/registration-tab/registration-tab.component';
import { PlayersListTabComponent } from './Tabs/players-list-tab/players-list-tab.component';


@NgModule({
    imports: [
        BrowserModule, FormsModule, AppRoutingModule,
        ReactiveFormsModule, HttpClientModule, MaterialCustomModule,
    ],
    declarations: [AppComponent, ScoresTabComponent, MyAccountTabComponent,
        CompetitionTabComponent, OfferTabComponent, ContactTabComponent,
        AboutUsTabComponent, NavbarComponent, CompetitionsSelectComponent, RegistrationTabComponent, PlayersListTabComponent],
    providers: [
        CompetitionService, MessageService
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
