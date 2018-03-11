import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompetitionsSelectComponent } from './competitions-select.component';

describe('CompetitionsSelectComponent', () => {
  let component: CompetitionsSelectComponent;
  let fixture: ComponentFixture<CompetitionsSelectComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompetitionsSelectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompetitionsSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
