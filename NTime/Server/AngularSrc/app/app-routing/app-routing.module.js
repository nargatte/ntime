"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var common_1 = require("@angular/common");
var router_1 = require("@angular/router");
var about_us_tab_component_1 = require("../about-us-tab/about-us-tab.component");
var contact_tab_component_1 = require("../contact-tab/contact-tab.component");
var my_account_tab_component_1 = require("../my-account-tab/my-account-tab.component");
var offer_tab_component_1 = require("../offer-tab/offer-tab.component");
var registration_tab_component_1 = require("../registration-tab/registration-tab.component");
var scores_tab_component_1 = require("../scores-tab/scores-tab.component");
var material_1 = require("@angular/material");
var animations_1 = require("@angular/platform-browser/animations");
//import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
var routes = [
    { path: 'about', component: about_us_tab_component_1.AboutUsTabComponent, data: { name: 'about' } },
    { path: 'contact', component: contact_tab_component_1.ContactTabComponent, data: { name: 'contact' } },
    { path: 'account', component: my_account_tab_component_1.MyAccountTabComponent, data: { name: 'account' } },
    { path: 'offer', component: offer_tab_component_1.OfferTabComponent, data: { name: 'offer' } },
    { path: 'registration', component: registration_tab_component_1.RegistrationTabComponent, data: { name: 'registration' } },
    { path: 'scores', component: scores_tab_component_1.ScoresTabComponent, data: { name: 'scores' } },
    { path: '', redirectTo: '/about', pathMatch: 'full' },
];
var AppRoutingModule = /** @class */ (function () {
    function AppRoutingModule() {
    }
    AppRoutingModule = __decorate([
        core_1.NgModule({
            imports: [
                router_1.RouterModule.forRoot(routes),
                animations_1.BrowserAnimationsModule, material_1.MatButtonModule //Material
                //NgbModule,
            ],
            exports: [router_1.RouterModule],
            providers: [{ provide: common_1.APP_BASE_HREF, useValue: '/' }]
        })
    ], AppRoutingModule);
    return AppRoutingModule;
}());
exports.AppRoutingModule = AppRoutingModule;
//# sourceMappingURL=app-routing.module.js.map