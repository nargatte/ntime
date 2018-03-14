import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewPlayerInputComponent } from './new-player-input.component';

describe('NewPlayerInputComponent', () => {
  let component: NewPlayerInputComponent;
  let fixture: ComponentFixture<NewPlayerInputComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewPlayerInputComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewPlayerInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
