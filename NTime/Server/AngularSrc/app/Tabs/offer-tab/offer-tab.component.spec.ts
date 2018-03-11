import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OfferTabComponent } from './offer-tab.component';

describe('OfferTabComponent', () => {
  let component: OfferTabComponent;
  let fixture: ComponentFixture<OfferTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OfferTabComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OfferTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
