import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SingUpEndDateErrorDialogComponent } from './sing-up-end-date-error-dialog.component';

describe('SingUpDateErrorDialogComponent', () => {
  let component: SingUpEndDateErrorDialogComponent;
  let fixture: ComponentFixture<SingUpEndDateErrorDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SingUpEndDateErrorDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SingUpEndDateErrorDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
