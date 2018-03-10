import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
//import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import { MatButtonModule, MatTabsModule, MatIconModule } from '@angular/material'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'

import { AppComponent } from './app.component';
import { ScoresTabComponent } from './scores-tab/scores-tab.component';
import { MyAccountTabComponent } from './my-account-tab/my-account-tab.component';
import { RegistrationTabComponent } from './registration-tab/registration-tab.component';
import { OfferTabComponent } from './offer-tab/offer-tab.component';
import { ContactTabComponent } from './contact-tab/contact-tab.component';
import { AboutUsTabComponent } from './about-us-tab/about-us-tab.component';
import { AppRoutingModule } from './app-routing/app-routing.module';
import { TabRouterComponent } from './tab-router/tab-router.component';

@NgModule({
    imports: [
        BrowserModule, AppRoutingModule,
        //NgbModule.forRoot(),
        FormsModule,
        BrowserAnimationsModule, MatButtonModule, MatTabsModule, MatIconModule //Material
    ],
    declarations: [AppComponent, ScoresTabComponent, MyAccountTabComponent, RegistrationTabComponent, OfferTabComponent, ContactTabComponent, AboutUsTabComponent, TabRouterComponent],
    bootstrap: [AppComponent]
})
export class AppModule { }
