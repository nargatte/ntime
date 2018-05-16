import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SuccessfullActionDialogComponent } from './successfull-action-dialog.component';
import { AppModule } from '../../../app.module';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

describe('SuccessfullActionDialogComponent', () => {
  let component: SuccessfullActionDialogComponent;
  let fixture: ComponentFixture<SuccessfullActionDialogComponent>;

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
    fixture = TestBed.createComponent(SuccessfullActionDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
