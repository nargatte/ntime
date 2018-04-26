import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayersTabHeaderAdminComponent } from './players-tab-header-admin.component';

describe('PlayersTabHeaderAdminComponent', () => {
  let component: PlayersTabHeaderAdminComponent;
  let fixture: ComponentFixture<PlayersTabHeaderAdminComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlayersTabHeaderAdminComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayersTabHeaderAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
