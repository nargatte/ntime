import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayerAddedDialogComponent } from './player-added-dialog.component';
import { AppModule } from '../../../app.module';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

describe('PlayerAddedDialogComponent', () => {
  let component: PlayerAddedDialogComponent;
  let fixture: ComponentFixture<PlayerAddedDialogComponent>;

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
    fixture = TestBed.createComponent(PlayerAddedDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
