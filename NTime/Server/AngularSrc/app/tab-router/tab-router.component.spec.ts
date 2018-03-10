import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TabRouterComponent } from './tab-router.component';

describe('TabRouterComponent', () => {
  let component: TabRouterComponent;
  let fixture: ComponentFixture<TabRouterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TabRouterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TabRouterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
