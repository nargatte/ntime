import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrationTabComponent } from './registration-tab.component';
import { MaterialCustomModule } from '../../Modules/material-custom.module';
import { AppModule } from '../../app.module';

describe('RegistrationTabComponent', () => {
  let component: RegistrationTabComponent;
  let fixture: ComponentFixture<RegistrationTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrationTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
