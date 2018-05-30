import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SingUpEndDateErrorDialogComponent } from './sing-up-end-date-error-dialog.component';
import { AppModule } from '../../../app.module';

describe('SingUpDateErrorDialogComponent', () => {
  let component: SingUpEndDateErrorDialogComponent;
  let fixture: ComponentFixture<SingUpEndDateErrorDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule]
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
