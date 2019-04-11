import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PasswordRequirementsInfoComponent } from './password-requirements-info.component';

describe('PasswordRequirementsInfoComponent', () => {
  let component: PasswordRequirementsInfoComponent;
  let fixture: ComponentFixture<PasswordRequirementsInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PasswordRequirementsInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PasswordRequirementsInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
