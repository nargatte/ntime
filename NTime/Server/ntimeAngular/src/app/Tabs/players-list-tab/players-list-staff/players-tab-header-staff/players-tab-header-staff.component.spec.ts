import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayersTabHeaderStaffComponent } from './players-tab-header-staff.component';
import { AppModule } from '../../../../app.module';
import { Competition } from '../../../../Models/Competitions/Competition';

describe('PlayersTabHeaderAdminComponent', () => {
  let component: PlayersTabHeaderStaffComponent;
  let fixture: ComponentFixture<PlayersTabHeaderStaffComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayersTabHeaderStaffComponent);
    component = fixture.componentInstance;
    component.competition = new Competition();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
