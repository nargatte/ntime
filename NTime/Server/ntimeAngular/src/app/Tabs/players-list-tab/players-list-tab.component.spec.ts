import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayersListTabComponent } from './players-list-tab.component';

describe('PlayersListTabComponent', () => {
  let component: PlayersListTabComponent;
  let fixture: ComponentFixture<PlayersListTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlayersListTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayersListTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
