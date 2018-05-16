import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompetitionTabComponent } from './competition-tab.component';
import { MaterialCustomModule } from '../../Modules/material-custom.module';
import { AppModule } from '../../app.module';

describe('CompetitionTabComponent', () => {
  let component: CompetitionTabComponent;
  let fixture: ComponentFixture<CompetitionTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompetitionTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
