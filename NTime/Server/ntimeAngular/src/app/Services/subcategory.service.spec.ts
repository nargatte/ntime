import { TestBed, inject } from '@angular/core/testing';

import { SubcategoryService } from './subcategory.service';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { MessageService } from './message.service';
import { HttpTestingController, HttpClientTestingModule } from '@angular/common/http/testing';
import { Subcategory } from '../Models/Subcategory';
import { Distance } from '../Models/Distance';

describe('SubcategoryService', () => {
  let subcategoryService: SubcategoryService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ HttpClientTestingModule ],
      providers: [SubcategoryService, MessageService]
    });

    subcategoryService = TestBed.get(SubcategoryService);
    httpMock = TestBed.get(HttpTestingController);
  });

  it('should be created', inject([SubcategoryService], (service: SubcategoryService) => {
    expect(service).toBeTruthy();
  }));

  it('getSubcategoryFromCompetition-return-instanceof(Subcategory[])', (done) => {
    subcategoryService.getSubcategoryFromCompetition(0).subscribe((res: Subcategory[]) => {
      expect(res).toEqual(jasmine.any(Array));
      expect(res[0]).toEqual(jasmine.any(Subcategory));
      done();
    });
    const subcategory = [new Subcategory()];
    const subcategoryRequest = httpMock.expectOne('/api/Subcategory/FromCompetition/0');
    subcategoryRequest.flush(subcategory);

    httpMock.verify();

  });
});
