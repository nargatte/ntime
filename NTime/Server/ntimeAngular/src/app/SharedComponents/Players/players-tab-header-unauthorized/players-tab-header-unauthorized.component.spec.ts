import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayersTabHeaderUnauthorizedComponent } from './players-tab-header-unauthorized.component';
import { AppModule } from '../../../app.module';
import { Competition } from '../../../Models/Competition';
import { MockCompetition } from '../../../MockData/MockCompetition';

describe('PlayersTabHeaderUnauthorizedComponent', () => {
  let component: PlayersTabHeaderUnauthorizedComponent;
  let fixture: ComponentFixture<PlayersTabHeaderUnauthorizedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule],
      providers: [
        { provide: Competition, useValue: { MockCompetition } }
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayersTabHeaderUnauthorizedComponent);
    component = fixture.componentInstance;
    component.competition = new Competition(1, 'Kolumna', new Date(Date.now()), new Date(Date.now()));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
