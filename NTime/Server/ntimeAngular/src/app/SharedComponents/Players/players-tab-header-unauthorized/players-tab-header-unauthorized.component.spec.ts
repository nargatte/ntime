import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayersTabHeaderUnauthorizedComponent } from './players-tab-header-unauthorized.component';

describe('PlayersTabHeaderUnauthorizedComponent', () => {
  let component: PlayersTabHeaderUnauthorizedComponent;
  let fixture: ComponentFixture<PlayersTabHeaderUnauthorizedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlayersTabHeaderUnauthorizedComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayersTabHeaderUnauthorizedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
