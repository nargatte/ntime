import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrationTabComponent } from './registration-tab.component';
import { MaterialCustomModule } from '../../Modules/material-custom.module';
import { AppModule } from '../../app.module';
import { Competition } from '../../Models/Competition';
import { Distance } from '../../Models/Distance';
import { Subcategory } from '../../Models/Subcategory';
import { MockCompetitions } from '../../MockData/mockCompetitions';

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
    component.competition = MockCompetitions[0];
    component.distances = [new Distance()];
    component.subcategories = [new Subcategory()];
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
