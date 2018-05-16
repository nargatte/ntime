import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LogInFormComponent } from './log-in-form.component';
import { SuccessfullActionDialogComponent } from '../../Dialogs/successfull-action-dialog/successfull-action-dialog.component';
import { FailedActionDialogComponent } from '../../Dialogs/failed-action-dialog/failed-action-dialog.component';
import { MaterialCustomModule } from '../../../Modules/material-custom.module';
import { FormsModule } from '@angular/forms';
import { AuthenticatedUserService } from '../../../Services/authenticated-user.service';
import { MessageService } from '../../../Services/message.service';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AuthenticationService } from '../../../Services/authentication.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('LogInFormComponent', () => {
  let component: LogInFormComponent;
  let fixture: ComponentFixture<LogInFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [FormsModule, MaterialCustomModule, HttpClientModule, BrowserAnimationsModule],
      declarations: [ LogInFormComponent, SuccessfullActionDialogComponent, FailedActionDialogComponent ],
      providers: [AuthenticationService, HttpClient, MessageService, AuthenticatedUserService]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LogInFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
