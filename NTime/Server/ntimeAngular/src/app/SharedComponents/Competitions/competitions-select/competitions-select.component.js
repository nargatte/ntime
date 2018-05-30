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
var competition_service_1 = require("../../Services/competition.service");
var CompetitionsSelectComponent = /** @class */ (function () {
    function CompetitionsSelectComponent(competitionService) {
        this.competitionService = competitionService;
    }
    CompetitionsSelectComponent.prototype.ngOnInit = function () {
        this.getHeroes();
    };
    CompetitionsSelectComponent.prototype.getHeroes = function () {
        var _this = this;
        this.competitionService.getCompetitions().subscribe(function (competitions) { return _this.competitions = competitions; });
    };
    CompetitionsSelectComponent = __decorate([
        core_1.Component({
            selector: 'app-competitions-select',
            templateUrl: './competitions-select.component.html',
            styleUrls: ['./competitions-select.component.css']
        }),
        __metadata("design:paramtypes", [competition_service_1.CompetitionService])
    ], CompetitionsSelectComponent);
    return CompetitionsSelectComponent;
}());
exports.CompetitionsSelectComponent = CompetitionsSelectComponent;
//# sourceMappingURL=competitions-select.component.js.map