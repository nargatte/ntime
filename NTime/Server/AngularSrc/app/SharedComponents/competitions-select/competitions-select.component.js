"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var forms_1 = require("@angular/forms");
//import { Competition } from '../../Models/Competition';
var CompetitionsSelectComponent = /** @class */ (function () {
    function CompetitionsSelectComponent() {
        this.animalControl = new forms_1.FormControl('', [forms_1.Validators.required]);
        this.animals = [
            { name: 'Dog', sound: 'Woof!' },
            { name: 'Cat', sound: 'Meow!' },
            { name: 'Cow', sound: 'Moo!' },
            { name: 'Fox', sound: 'Wa-pa-pa-pa-pa-pa-pow!' },
        ];
    }
    CompetitionsSelectComponent = __decorate([
        core_1.Component({
            selector: 'app-competitions-select',
            templateUrl: './competitions-select.component.html',
            styleUrls: ['./competitions-select.component.css']
        })
    ], CompetitionsSelectComponent);
    return CompetitionsSelectComponent;
}());
exports.CompetitionsSelectComponent = CompetitionsSelectComponent;
//# sourceMappingURL=competitions-select.component.js.map