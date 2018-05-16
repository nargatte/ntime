import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayersListAdminComponent } from './players-list-admin.component';
import { AppModule } from '../../../app.module';

describe('PlayersListAdminComponent', () => {
  let component: PlayersListAdminComponent;
  let fixture: ComponentFixture<PlayersListAdminComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayersListAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
