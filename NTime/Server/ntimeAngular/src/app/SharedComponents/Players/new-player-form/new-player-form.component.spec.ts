import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewPlayerFormComponent } from './new-player-form.component';
import { AppModule } from '../../../app.module';

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
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
