import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ForgotPasswordTabComponent } from './forgot-password-tab.component';

describe('ForgotPasswordTabComponent', () => {
  let component: ForgotPasswordTabComponent;
  let fixture: ComponentFixture<ForgotPasswordTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ForgotPasswordTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ForgotPasswordTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
