import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScoresTabComponent } from './scores-tab.component';
import { MaterialCustomModule } from '../../Modules/material-custom.module';
import { AppModule } from '../../app.module';

describe('ScoresComponent', () => {
  let component: ScoresTabComponent;
  let fixture: ComponentFixture<ScoresTabComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [AppModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScoresTabComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
