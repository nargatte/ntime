import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FailedActionDialogComponent } from './failed-action-dialog.component';

describe('FailedActionDialogComponent', () => {
  let component: FailedActionDialogComponent;
  let fixture: ComponentFixture<FailedActionDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FailedActionDialogComponent ]
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
