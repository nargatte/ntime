import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayersTabHeaderUnauthorizedComponent } from './players-tab-header-unauthorized.component';
import { AppModule } from '../../../../app.module';
import { Competition } from '../../../../Models/Competitions/Competition';
import { MockCompetitions } from '../../../../MockData/mockCompetitions';

describe('PlayersTabHeaderUnauthorizedComponent', () => {
  let component: PlayersTabHeaderUnauthorizedComponent;
  let fixture: ComponentFixture<PlayersTabHeaderUnauthorizedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule],
      providers: [
        { provide: Competition, useValue: { MockCompetitions } }
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayersTabHeaderUnauthorizedComponent);
    component = fixture.componentInstance;
    component.competition = new Competition();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
