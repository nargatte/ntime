import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewPlayerFormComponent } from './new-player-form.component';
import { AppModule } from '../../../app.module';
import { Subcategory } from '../../../Models/Subcategory';
import { Distance } from '../../../Models/Distance';
import { Competition } from '../../../Models/Competitions/Competition';

describe('NewPlayerFormComponent', () => {
  let component: NewPlayerFormComponent;
  let fixture: ComponentFixture<NewPlayerFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewPlayerFormComponent);
    component = fixture.componentInstance;
    component.subcategories = [new Subcategory()];
    component.distances = [new Distance()];
    component.competition = new Competition(1, 'Kolumna', new Date(Date.now()), new Date(Date.now()));

    window['grecaptcha'] = { 'render': function(s: string) {
      return 0;
    } };


    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
