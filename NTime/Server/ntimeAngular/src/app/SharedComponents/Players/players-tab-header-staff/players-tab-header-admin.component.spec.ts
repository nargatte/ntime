import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayersTabHeaderAdminComponent } from './players-tab-header-admin.component';
import { AppModule } from '../../../app.module';
import { Competition } from '../../../Models/Competition';

describe('PlayersTabHeaderAdminComponent', () => {
  let component: PlayersTabHeaderAdminComponent;
  let fixture: ComponentFixture<PlayersTabHeaderAdminComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayersTabHeaderAdminComponent);
    component = fixture.componentInstance;
    component.competition = new Competition(1, 'Kolumna', new Date(Date.now()), new Date(Date.now()));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
