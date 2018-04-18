import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SuccessfullActionDialogComponent } from './successfull-action-dialog.component';

describe('SuccessfullActionDialogComponent', () => {
  let component: SuccessfullActionDialogComponent;
  let fixture: ComponentFixture<SuccessfullActionDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SuccessfullActionDialogComponent ]
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
