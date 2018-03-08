import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScoresTabComponent } from './scores-tab.component';

describe('ScoresComponent', () => {
  let component: ScoresTabComponent;
  let fixture: ComponentFixture<ScoresTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScoresTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScoresTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
