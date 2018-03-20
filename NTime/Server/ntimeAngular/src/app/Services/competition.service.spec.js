"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var testing_1 = require("@angular/core/testing");
var competition_service_1 = require("./competition.service");
describe('CompetitionService', function () {
    beforeEach(function () {
        testing_1.TestBed.configureTestingModule({
            providers: [competition_service_1.CompetitionService]
        });
    });
    it('should be created', testing_1.inject([competition_service_1.CompetitionService], function (service) {
        expect(service).toBeTruthy();
    }));
});
//# sourceMappingURL=competition.service.spec.js.map