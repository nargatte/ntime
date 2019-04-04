import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayersListStaffComponent } from './players-list-staff.component';
import { AppModule } from '../../../app.module';

describe('PlayersListAdminComponent', () => {
  let component: PlayersListStaffComponent;
  let fixture: ComponentFixture<PlayersListStaffComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayersListStaffComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
