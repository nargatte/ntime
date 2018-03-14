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

const routes: Routes = [
    { path: 'o-nas', component: AboutUsTabComponent, data: { name: 'about' } },
    { path: 'kontakt', component: ContactTabComponent, data: { name: 'contact' } },
    { path: 'konto', component: MyAccountTabComponent, data: { name: 'account' } },
    { path: 'oferta', component: OfferTabComponent, data: { name: 'offer' } },
    { path: 'zawody', component: RegistrationTabComponent, data: { name: 'competitions' } },
    { path: 'wyniki', component: ScoresTabComponent, data: { name: 'scores' } },
    { path: '', redirectTo: '/o-nas', pathMatch:'full' },
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
