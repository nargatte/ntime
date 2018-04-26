import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayersSelectComponent } from './players-select.component';

describe('PlayersSelectComponent', () => {
  let component: PlayersSelectComponent;
  let fixture: ComponentFixture<PlayersSelectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlayersSelectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayersSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
