import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewPasswordTabComponent } from './new-password-tab.component';

describe('NewPasswordTabComponent', () => {
  let component: NewPasswordTabComponent;
  let fixture: ComponentFixture<NewPasswordTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewPasswordTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewPasswordTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
