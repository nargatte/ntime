import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayersListTabComponent } from './players-list-tab.component';
import { MaterialCustomModule } from '../../Modules/material-custom.module';
import { AppModule } from '../../app.module';

describe('PlayersListTabComponent', () => {
  let component: PlayersListTabComponent;
  let fixture: ComponentFixture<PlayersListTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayersListTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
