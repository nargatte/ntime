import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserRegisteredDialogComponent } from './user-registered-dialog.component';

describe('UserRegisteredDialogComponent', () => {
  let component: UserRegisteredDialogComponent;
  let fixture: ComponentFixture<UserRegisteredDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserRegisteredDialogComponent ]
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
