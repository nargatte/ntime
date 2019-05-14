import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OldWebsiteLinkComponent } from './old-website-link.component';

describe('OldWebsiteLinkComponent', () => {
  let component: OldWebsiteLinkComponent;
  let fixture: ComponentFixture<OldWebsiteLinkComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OldWebsiteLinkComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OldWebsiteLinkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
