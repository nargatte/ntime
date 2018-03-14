"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var of_1 = require("rxjs/observable/of");
var operators_1 = require("rxjs/operators");
var http_1 = require("@angular/common/http");
var message_service_1 = require("../Services/message.service");
var CompetitionService = /** @class */ (function () {
    function CompetitionService(http, messageService) {
        this.http = http;
        this.messageService = messageService;
        this.baseCompetitionUrl = "http://testing.time2win.aspnet.pl/api/Competition";
        this.getCompetitionsUrl = this.baseCompetitionUrl + "?ItemsOnPage=10&PageNumber=0";
        this.httpOptions = {
            headers: new http_1.HttpHeaders({ 'Content-Type': 'application/json' })
        };
    }
    CompetitionService.prototype.getCompetitions = function () {
        var _this = this;
        return this.http.get(this.getCompetitionsUrl).pipe(operators_1.tap(function (competitions) { return _this.log('Pobrano zawody'); }), operators_1.catchError(this.handleError('getCompetitions', [])));
    };
    CompetitionService.prototype.log = function (message) {
        this.messageService.addLog('CompetitionService: ' + message);
    };
    CompetitionService.prototype.handleError = function (operation, result) {
        var _this = this;
        if (operation === void 0) { operation = 'operation'; }
        return function (error) {
            //TODO: send the error to remote logging infrastructure
            console.error(error); // Right now only logging to console
            //TODO: better job of transforming error fo user consumption
            _this.log(operation + " failed: " + error.message);
            //Let the app keep running by returning an empty result.
            return of_1.of(result);
        };
    };
    CompetitionService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [http_1.HttpClient, message_service_1.MessageService])
    ], CompetitionService);
    return CompetitionService;
}());
exports.CompetitionService = CompetitionService;
//# sourceMappingURL=competition.service.js.map