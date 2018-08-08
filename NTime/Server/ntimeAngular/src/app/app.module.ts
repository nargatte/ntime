import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { APP_BASE_HREF, Location } from '@angular/common';
import { HttpClientModule, HttpClient } from '@angular/common/http';

// import { FlexLayoutModule } from '@angular/flex-layout';

import { AppRoutingModule } from './Modules/app-routing.module';
import { MaterialCustomModule } from './Modules/material-custom.module';
import { AuthenticationModule } from './Modules/authentication.module';
import { MAT_DATE_LOCALE } from '@angular/material';

import { AppComponent } from './app.component';
import { ScoresTabComponent } from './Tabs/scores-tab/scores-tab.component';
import { MyAccountTabComponent } from './Tabs/my-account-tab/my-account-tab.component';
import { CompetitionTabComponent } from './Tabs/competition-tab/competition-tab.component';
import { OfferTabComponent } from './Tabs/offer-tab/offer-tab.component';
import { ContactTabComponent } from './Tabs/contact-tab/contact-tab.component';
import { AboutUsTabComponent } from './Tabs/about-us-tab/about-us-tab.component';
import { NavbarComponent } from './SharedComponents/navbar/navbar.component';
import { CompetitionsSelectComponent } from './SharedComponents/Competitions/competitions-select/competitions-select.component';
import { CompetitionService } from './Services/competition.service';
import { MessageService } from './Services/message.service';
import { RegistrationTabComponent } from './Tabs/registration-tab/registration-tab.component';
import { PlayersListTabComponent } from './Tabs/players-list-tab/players-list-tab.component';
import { NewPlayerFormComponent } from './SharedComponents/Players/new-player-form/new-player-form.component';
import { PlayerService } from './Services/player.service';
import { DistanceService } from './Services/distance.service';
import { SubcategoryService } from './Services/subcategory.service';
import { PlayersListComponent } from './SharedComponents/Players/players-list/players-list.component';
import { PlayerAddedDialogComponent } from './SharedComponents/Dialogs/player-added-dialog/player-added-dialog.component';
import {
    SingUpEndDateErrorDialogComponent
} from './SharedComponents/Dialogs/sing-up-end-date-error-dialog/sing-up-end-date-error-dialog.component';
import { RegisterConfirmationComponent } from './SharedComponents/Accounts/register-confirmation/register-confirmation.component';
import { AuthenticationService } from './Services/authentication.service';
import { UserRegisteredDialogComponent } from './SharedComponents/Dialogs/user-registered-dialog/user-registered-dialog.component';
import { SuccessfullActionDialogComponent } from './SharedComponents/Dialogs/successfull-action-dialog/successfull-action-dialog.component';
import { FailedActionDialogComponent } from './SharedComponents/Dialogs/failed-action-dialog/failed-action-dialog.component';
import { AuthenticatedUserService } from './Services/authenticated-user.service';
import { PlayersSelectComponent } from './SharedComponents/Players/players-select/players-select.component';
import { PlayersListUnauthorizedComponent } from './SharedComponents/Players/players-list-unauthorized/players-list-unauthorized.component';
import { PlayersListStaffComponent } from './SharedComponents/Players/players-list-staff/players-list-staff.component';
// tslint:disable-next-line:max-line-length
import { PlayersTabHeaderUnauthorizedComponent } from './SharedComponents/Players/players-tab-header-unauthorized/players-tab-header-unauthorized.component';
import { PlayersTabHeaderStaffComponent } from './SharedComponents/Players/players-tab-header-staff/players-tab-header-staff.component';
import { OrganizerAccountService } from './Services/organizer-account.service';
import { EditPlayerComponent } from './SharedComponents/Players/edit-player/edit-player.component';
import { EditPlayerTabComponent } from './Tabs/edit-player-tab/edit-player-tab.component';
import { CompetitionDetailsComponent } from './SharedComponents/Competitions/competition-details/competition-details.component';


@NgModule({
    imports: [
        AppRoutingModule, AuthenticationModule, BrowserModule, FormsModule,
        HttpClientModule, MaterialCustomModule, ReactiveFormsModule
    ],
    declarations: [
        AboutUsTabComponent, AppComponent, CompetitionsSelectComponent, ContactTabComponent,
        CompetitionTabComponent, FailedActionDialogComponent, MyAccountTabComponent, NavbarComponent,
        NewPlayerFormComponent, OfferTabComponent, PlayerAddedDialogComponent, PlayersListStaffComponent,
        PlayersListComponent, PlayersListTabComponent, PlayersListUnauthorizedComponent,
        PlayersSelectComponent, PlayersTabHeaderStaffComponent, PlayersTabHeaderUnauthorizedComponent,
        RegisterConfirmationComponent, RegistrationTabComponent, ScoresTabComponent,
        SingUpEndDateErrorDialogComponent, SuccessfullActionDialogComponent, UserRegisteredDialogComponent, EditPlayerComponent, EditPlayerTabComponent, CompetitionDetailsComponent
    ],
    providers: [
        AuthenticatedUserService, AuthenticationService, CompetitionService, DistanceService,
        MessageService, OrganizerAccountService, PlayerService, SubcategoryService,
        { provide: MAT_DATE_LOCALE, useValue: 'pl-pl' },
    ],
    entryComponents: [
        FailedActionDialogComponent, PlayerAddedDialogComponent, SingUpEndDateErrorDialogComponent,
        SuccessfullActionDialogComponent, UserRegisteredDialogComponent
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
