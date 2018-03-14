import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AboutUsTabComponent } from './about-us-tab.component';

describe('AboutUsTabComponent', () => {
  let component: AboutUsTabComponent;
  let fixture: ComponentFixture<AboutUsTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AboutUsTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AboutUsTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
