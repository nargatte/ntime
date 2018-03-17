import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SimplePlayerListComponent } from './simple-player-list.component';

describe('SimplePlayerListComponent', () => {
  let component: SimplePlayerListComponent;
  let fixture: ComponentFixture<SimplePlayerListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SimplePlayerListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SimplePlayerListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
