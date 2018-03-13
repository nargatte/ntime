import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
//import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
    MatButtonModule, MatTabsModule, MatIconModule, MatToolbarModule,
} from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


//import { FlexLayoutModule } from '@angular/flex-layout';

import { AppComponent } from './app.component';
import { ScoresTabComponent } from './Tabs/scores-tab/scores-tab.component';
import { MyAccountTabComponent } from './Tabs/my-account-tab/my-account-tab.component';
import { RegistrationTabComponent } from './Tabs/registration-tab/registration-tab.component';
import { OfferTabComponent } from './Tabs/offer-tab/offer-tab.component';
import { ContactTabComponent } from './Tabs/contact-tab/contact-tab.component';
import { AboutUsTabComponent } from './Tabs/about-us-tab/about-us-tab.component';
import { AppRoutingModule } from './app-routing/app-routing.module';
import { NavbarComponent } from './navbar/navbar.component';
import { CompetitionsSelectComponent } from './SharedComponents/competitions-select/competitions-select.component';


@NgModule({
    imports: [
        BrowserModule, AppRoutingModule, ReactiveFormsModule,
        //NgbModule.forRoot(),
        FormsModule,
        BrowserAnimationsModule, MatButtonModule, MatTabsModule, MatIconModule, //Material
        MatToolbarModule//Material 
    ],
    declarations: [AppComponent, ScoresTabComponent, MyAccountTabComponent,
        RegistrationTabComponent, OfferTabComponent, ContactTabComponent,
        AboutUsTabComponent, NavbarComponent, CompetitionsSelectComponent],
    bootstrap: [AppComponent]
})
export class AppModule {
}
