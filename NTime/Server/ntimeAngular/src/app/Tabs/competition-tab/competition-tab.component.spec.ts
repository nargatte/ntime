import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompetitionTabComponent } from './competition-tab.component';

describe('CompetitionTabComponent', () => {
  let component: CompetitionTabComponent;
  let fixture: ComponentFixture<CompetitionTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompetitionTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompetitionTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
