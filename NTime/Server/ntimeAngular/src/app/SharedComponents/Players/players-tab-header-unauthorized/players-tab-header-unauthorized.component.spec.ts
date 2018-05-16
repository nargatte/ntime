import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayersTabHeaderUnauthorizedComponent } from './players-tab-header-unauthorized.component';
import { AppModule } from '../../../app.module';

describe('PlayersTabHeaderUnauthorizedComponent', () => {
  let component: PlayersTabHeaderUnauthorizedComponent;
  let fixture: ComponentFixture<PlayersTabHeaderUnauthorizedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule]
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
