import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayerAddedDialogComponent } from './player-added-dialog.component';

describe('PlayerAddedDialogComponent', () => {
  let component: PlayerAddedDialogComponent;
  let fixture: ComponentFixture<PlayerAddedDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlayerAddedDialogComponent ]
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
