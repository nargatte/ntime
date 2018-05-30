import { UrlBuilder } from './url-builder';
import { IUrlBuilder } from './IUrlBuilder';
import { TestBed } from '@angular/core/testing';

describe('UrlBuilder', () => {
  let urlBuilder: IUrlBuilder;
  let initialUrl: string;

  beforeEach(() => {

    TestBed.configureTestingModule({
      providers: [UrlBuilder]
    });
    urlBuilder = TestBed.get(UrlBuilder);
    initialUrl = urlBuilder.toString();
  });

  it('should create an instance', () => {
    expect(new UrlBuilder()).toBeTruthy();
  });

  it('addControllerName-create-correctUrl', () => {
    const controllerName = 'TestController';
    const createdUrl =
      urlBuilder
        .addControllerName(controllerName)
        .toString();
    expect(createdUrl).toEqual(`${initialUrl}/api/${controllerName}`);
  });

  it('addCustomUrlPart-create-correctUrl', () => {
    const urlPart = 'SomeTestUrl';
    const createdUrl =
      urlBuilder
        .addCustomUrlPart(urlPart)
        .toString();
    expect(createdUrl).toEqual(`${initialUrl}${urlPart}`);
  });

  it('addId-create-correctUrl', () => {
    const sampleId = 5;
    const createdUrl =
      urlBuilder
        .addId(sampleId)
        .toString();
    expect(createdUrl).toEqual(`${initialUrl}/${sampleId}`);
  });

  it('addPageRequest-create-correctUrl', () => {
    const pageSize = 20;
    const pageNumber = 5;
    const createdUrl =
      urlBuilder
        .addPageRequest(pageSize, pageNumber)
        .toString();
    expect(createdUrl).toEqual(`${initialUrl}?ItemsOnPage=${pageSize}&PageNumber=${pageNumber}`);
  });

  it('combinerUrlFunctions-create-correctUrl', () => {
    const urlPart = '/SampleUrlPart31241';
    const sampleId = 7;
    const controllerName = 'TestController';
    const pageSize = 20;
    const pageNumber = 5;
    const createdUrl =
      urlBuilder
        .addControllerName(controllerName)
        .addId(sampleId)
        .addCustomUrlPart(urlPart)
        .addPageRequest(pageSize, pageNumber)
        .toString();
    expect(createdUrl).toEqual(
      `${initialUrl}/api/${controllerName}/${sampleId}${urlPart}?ItemsOnPage=${pageSize}&PageNumber=${pageNumber}`
    );
  });


});
