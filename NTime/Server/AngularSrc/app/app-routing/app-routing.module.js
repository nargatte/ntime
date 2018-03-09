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
var ng_bootstrap_1 = require("@ng-bootstrap/ng-bootstrap");
var routes = [
    { path: 'about', component: about_us_tab_component_1.AboutUsTabComponent },
    { path: 'contact', component: contact_tab_component_1.ContactTabComponent },
    { path: 'account', component: my_account_tab_component_1.MyAccountTabComponent },
    { path: 'offer', component: offer_tab_component_1.OfferTabComponent },
    { path: 'registration', component: registration_tab_component_1.RegistrationTabComponent },
    { path: 'scores', component: scores_tab_component_1.ScoresTabComponent },
    { path: '', redirectTo: '/about', pathMatch: 'full' },
];
var AppRoutingModule = /** @class */ (function () {
    function AppRoutingModule() {
    }
    AppRoutingModule = __decorate([
        core_1.NgModule({
            imports: [
                router_1.RouterModule.forRoot(routes),
                ng_bootstrap_1.NgbModule,
            ],
            exports: [router_1.RouterModule],
            providers: [{ provide: common_1.APP_BASE_HREF, useValue: '/' }]
        })
    ], AppRoutingModule);
    return AppRoutingModule;
}());
exports.AppRoutingModule = AppRoutingModule;
//# sourceMappingURL=app-routing.module.js.map