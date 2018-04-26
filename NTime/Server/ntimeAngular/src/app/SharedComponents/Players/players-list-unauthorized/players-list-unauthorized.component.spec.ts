import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayersListUnauthorizedComponent } from './players-list-unauthorized.component';

describe('PlayersListUnauthorizedComponent', () => {
  let component: PlayersListUnauthorizedComponent;
  let fixture: ComponentFixture<PlayersListUnauthorizedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlayersListUnauthorizedComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayersListUnauthorizedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
