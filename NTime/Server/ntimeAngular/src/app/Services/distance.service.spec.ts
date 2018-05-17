import { TestBed, inject } from '@angular/core/testing';

import { DistanceService } from './distance.service';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { MessageService } from './message.service';
import { Distance } from '../Models/Distance';
import { HttpResponse } from 'selenium-webdriver/http';
import { Subcategory } from '../Models/ExtraPlayerInfo';

fdescribe('DistanceService', () => {
  let distanceService: DistanceService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [DistanceService, MessageService]
    });

    distanceService = TestBed.get(DistanceService);
    httpMock = TestBed.get(HttpTestingController);

  });

  it('should be created', inject([DistanceService], (service: DistanceService) => {
    expect(service).toBeTruthy();
  }));


  it('getDistanceFromCompetition-return-instanceof(Distance[])', (done) => {
    distanceService.getDistanceFromCompetition(0).subscribe((res: Distance[]) => {
      expect(res[0]).toEqual(jasmine.any(Distance));
      expect(res).toEqual(jasmine.any(Array));
      done();
    });
    const distances = [new Distance()];
    const distanceRequest = httpMock.expectOne('/api/Distance/FromCompetition/0');
    distanceRequest.flush(distances);

    httpMock.verify();
  });

});


