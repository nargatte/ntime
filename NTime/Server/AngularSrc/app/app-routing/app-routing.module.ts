import { NgModule } from '@angular/core';
import { APP_BASE_HREF } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { AboutUsTabComponent } from '../Tabs/about-us-tab/about-us-tab.component';
import { ContactTabComponent } from '../Tabs/contact-tab/contact-tab.component';
import { MyAccountTabComponent } from '../Tabs/my-account-tab/my-account-tab.component';
import { OfferTabComponent } from '../Tabs/offer-tab/offer-tab.component';
import { RegistrationTabComponent } from '../Tabs/registration-tab/registration-tab.component';
import { ScoresTabComponent } from '../Tabs/scores-tab/scores-tab.component';
import { MatButtonModule } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
//import { registerLocaleData } from '@angular/common'
//import localePl from '@angular/common/locales/pl';
//registerLocaleData(localePl, 'pl');

//import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

const routes: Routes = [
    { path: 'about', component: AboutUsTabComponent, data: { name: 'about' } },
    { path: 'contact', component: ContactTabComponent, data: { name: 'contact' } },
    { path: 'account', component: MyAccountTabComponent, data: { name: 'account' } },
    { path: 'offer', component: OfferTabComponent, data: { name: 'offer' } },
    { path: 'registration', component: RegistrationTabComponent, data: { name: 'registration' } },
    { path: 'scores', component: ScoresTabComponent, data: { name: 'scores' } },
    { path: '', redirectTo: '/about', pathMatch:'full' },
]

@NgModule({
    imports: [
        RouterModule.forRoot(routes),
        BrowserAnimationsModule, MatButtonModule //Material
        //NgbModule,
    ],
    exports: [RouterModule],
    providers: [
        { provide: APP_BASE_HREF, useValue: '/' },
    ]
})

export class AppRoutingModule { }
