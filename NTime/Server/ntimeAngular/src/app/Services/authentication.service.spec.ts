import { TestBed, inject } from '@angular/core/testing';

import { AuthenticationService } from './authentication.service';
import { MessageService } from './message.service';
import { AuthenticatedUserService } from './authenticated-user.service';
import { AppModule } from '../app.module';
import { HttpTestingController, HttpClientTestingModule } from '@angular/common/http/testing';
import { RoleViewModel } from '../Models/Authentication/RoleViewModel';
import { Distance } from '../Models/Distance';
import { TokenInfo } from '../Models/Authentication/TokenInfo';
import { RegisterBindingModel } from '../Models/Authentication/RegisterBindingModel';
import { RoleEnum } from '../Models/Enums/RoleEnum';

describe('AuthenticationService', () => {
  let authenticationService: AuthenticationService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ HttpClientTestingModule ],
      providers: [AuthenticationService, MessageService, AuthenticatedUserService]
    });

    authenticationService = TestBed.get(AuthenticationService);
    httpMock = TestBed.get(HttpTestingController);

  });

  it('should be created', inject([AuthenticationService], (service: AuthenticationService) => {
    expect(service).toBeTruthy();
  }));

  it('getRole-return-instanceof(RoleViewModel)', (done) => {
    authenticationService.getRole().subscribe((res: RoleViewModel) => {
      expect(res).toEqual(jasmine.any(RoleViewModel));
      done();
    });
    const role = new RoleViewModel();
    const distanceRequest = httpMock.expectOne('/api/Account/Role');
    distanceRequest.flush(role);

    httpMock.verify();
  });

  it('login-return-instanceof(TokenInfo)', (done) => {
    authenticationService.login(new URLSearchParams()).subscribe((res: TokenInfo) => {
      expect(res).toEqual(jasmine.any(TokenInfo));
      done();
    });
    const tokenInfo = new TokenInfo();
    const loginRequest = httpMock.expectOne('/Token');
    loginRequest.flush(tokenInfo);

    httpMock.verify();
  });

  it('logout-return-void', (done) => {
    authenticationService.logOut().subscribe((res: void) => {
      expect(res).toBeDefined();
      done();
    });
    const logoutRequest = httpMock.expectOne('/api/Account/Logout');
    logoutRequest.flush({});
    httpMock.verify();
  });

  it('registerUser-return-instanceof(RegisterBindingModel)', (done) => {
    const registerModel = new RegisterBindingModel();
    const role = RoleEnum.Organizer;

    authenticationService.registerUser(registerModel, role).subscribe((res: RegisterBindingModel) => {
      expect(res).toEqual(jasmine.any(RegisterBindingModel));
      done();
    });
    const registerRequest = httpMock.expectOne('/api/Account/Register');
    registerRequest.flush(registerModel);
    httpMock.verify();
  });
});
