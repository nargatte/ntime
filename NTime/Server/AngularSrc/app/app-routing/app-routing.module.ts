import { NgModule } from '@angular/core';
import { APP_BASE_HREF } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { AboutUsTabComponent } from '../about-us-tab/about-us-tab.component';
import { ContactTabComponent } from '../contact-tab/contact-tab.component';
import { MyAccountTabComponent } from '../my-account-tab/my-account-tab.component';
import { OfferTabComponent } from '../offer-tab/offer-tab.component';
import { RegistrationTabComponent } from '../registration-tab/registration-tab.component';
import { ScoresTabComponent } from '../scores-tab/scores-tab.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

const routes: Routes = [
    { path: 'about', component: AboutUsTabComponent },
    { path: 'contact', component: ContactTabComponent },
    { path: 'account', component: MyAccountTabComponent },
    { path: 'offer', component: OfferTabComponent },
    { path: 'registration', component: RegistrationTabComponent },
    { path: 'scores', component: ScoresTabComponent },
    { path: '', redirectTo: '/about', pathMatch:'full' },
]

@NgModule({
    imports: [
        RouterModule.forRoot(routes),
        NgbModule,
    ],
    exports: [RouterModule],
    providers: [{ provide: APP_BASE_HREF, useValue: '/' }]
})

export class AppRoutingModule { }
