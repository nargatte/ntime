import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AboutUsTabComponent } from './about-us-tab.component';
import { MaterialCustomModule } from '../../Modules/material-custom.module';
import { AppModule } from '../../app.module';

describe('AboutUsTabComponent', () => {
  let component: AboutUsTabComponent;
  let fixture: ComponentFixture<AboutUsTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule]
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
