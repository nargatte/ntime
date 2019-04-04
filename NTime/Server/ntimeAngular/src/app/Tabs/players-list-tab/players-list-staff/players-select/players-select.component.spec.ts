import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayersSelectComponent } from './players-select.component';
import { AppModule } from '../../../../app.module';

describe('PlayersSelectComponent', () => {
  let component: PlayersSelectComponent;
  let fixture: ComponentFixture<PlayersSelectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ AppModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayersSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
