import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserRegisteredDialogComponent } from './user-registered-dialog.component';
import { AppModule } from '../../../app.module';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

describe('UserRegisteredDialogComponent', () => {
  let component: UserRegisteredDialogComponent;
  let fixture: ComponentFixture<UserRegisteredDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ AppModule],
      providers: [
        { provide: MatDialogRef, useValue: {} },
        { provide: MAT_DIALOG_DATA, useValue: [] }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserRegisteredDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
