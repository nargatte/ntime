import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayersListUnauthorizedComponent } from './players-list-unauthorized.component';
import { AppModule } from '../../../app.module';

describe('PlayersListUnauthorizedComponent', () => {
  let component: PlayersListUnauthorizedComponent;
  let fixture: ComponentFixture<PlayersListUnauthorizedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ AppModule]
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
