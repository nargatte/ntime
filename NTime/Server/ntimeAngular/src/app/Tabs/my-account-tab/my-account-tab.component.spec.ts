import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MyAccountTabComponent } from './my-account-tab.component';
import { MaterialCustomModule } from '../../Modules/material-custom.module';
import { AppModule } from '../../app.module';

describe('MyAccountTabComponent', () => {
  let component: MyAccountTabComponent;
  let fixture: ComponentFixture<MyAccountTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MyAccountTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
