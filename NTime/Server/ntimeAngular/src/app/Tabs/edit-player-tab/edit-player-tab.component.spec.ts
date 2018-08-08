import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditPlayerTabComponent } from './edit-player-tab.component';

describe('EditPlayerTabComponent', () => {
  let component: EditPlayerTabComponent;
  let fixture: ComponentFixture<EditPlayerTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditPlayerTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditPlayerTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
