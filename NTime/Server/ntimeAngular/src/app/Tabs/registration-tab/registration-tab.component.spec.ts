import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrationTabComponent } from './registration-tab.component';
import { MaterialCustomModule } from '../../Modules/material-custom.module';
import { AppModule } from '../../app.module';
import { Competition } from '../../Models/Competition';

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
    component.competition = new Competition(1, 'Kolumna', new Date(Date.now()), new Date(Date.now()));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
