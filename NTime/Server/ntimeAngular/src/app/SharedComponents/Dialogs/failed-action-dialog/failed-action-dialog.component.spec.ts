import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FailedActionDialogComponent } from './failed-action-dialog.component';
import { AppModule } from '../../../app.module';
import { MaterialCustomModule } from '../../../Modules/material-custom.module';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

describe('FailedActionDialogComponent', () => {
  let component: FailedActionDialogComponent;
  let fixture: ComponentFixture<FailedActionDialogComponent>;

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
    fixture = TestBed.createComponent(FailedActionDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
