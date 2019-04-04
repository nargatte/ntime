import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterConfirmationComponent } from './register-confirmation.component';
import { AppRoutingModule } from '../../../Modules/app-routing.module';
import { AboutUsTabComponent } from '../../../Tabs/about-us-tab/about-us-tab.component';
import { ContactTabComponent } from '../../../Tabs/contact-tab/contact-tab.component';
import { MyAccountTabComponent } from '../../../Tabs/my-account-tab/my-account-tab.component';
import { OfferTabComponent } from '../../../Tabs/offer-tab/offer-tab.component';
import { PlayersListTabComponent } from '../../../Tabs/players-list-tab/players-list-tab.component';
import { RegistrationTabComponent } from '../../../Tabs/registration-tab/registration-tab.component';
import { ScoresTabComponent } from '../../../Tabs/scores-tab/scores-tab.component';
import { CompetitionTabComponent } from '../../../Tabs/competition-tab/competition-tab.component';
import { SuccessfullActionDialogComponent } from '../../Dialogs/successfull-action-dialog/successfull-action-dialog.component';
import { FailedActionDialogComponent } from '../../Dialogs/failed-action-dialog/failed-action-dialog.component';
import { FormsModule } from '@angular/forms';
import { MaterialCustomModule } from '../../../Modules/material-custom.module';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LogInFormComponent } from '../log-in-form/log-in-form.component';
import { RegisterFormComponent } from '../register-form/register-form.component';
import { UserRegisteredDialogComponent } from '../../Dialogs/user-registered-dialog/user-registered-dialog.component';
import { AppComponent } from '../../../app.component';
import { NavbarComponent } from '../../Navbar/navbar.component';
import { CompetitionsSelectComponent } from '../../Competitions/competitions-select/competitions-select.component';
import { NewPlayerFormComponent } from '../../Players/new-player-form/new-player-form.component';
import { PlayersListComponent } from '../../Players/players-list/players-list.component';
import { PlayerAddedDialogComponent } from '../../Dialogs/player-added-dialog/player-added-dialog.component';
import { SingUpEndDateErrorDialogComponent } from '../../Dialogs/sing-up-end-date-error-dialog/sing-up-end-date-error-dialog.component';
import { PlayersSelectComponent } from '../../Players/players-select/players-select.component';
import { PlayersListUnauthorizedComponent } from '../../../Tabs/players-list-tab/players-list-unauthorized/players-list-unauthorized.component';
import { PlayersListStaffComponent } from '../../../Tabs/players-list-tab/players-list-staff/players-list-staff.component';
// tslint:disable-next-line:max-line-length
import { PlayersTabHeaderUnauthorizedComponent } from '../../Players/players-tab-header-unauthorized/players-tab-header-unauthorized.component';
import { PlayersTabHeaderStaffComponent } from '../../Players/players-tab-header-staff/players-tab-header-staff.component';
import { AppModule } from '../../../app.module';
import { AuthenticationModule } from '../../../Modules/authentication.module';

describe('RegisterConfirmationComponent', () => {
  let component: RegisterConfirmationComponent;
  let fixture: ComponentFixture<RegisterConfirmationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule, AuthenticationModule],
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegisterConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
