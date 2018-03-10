"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var platform_browser_1 = require("@angular/platform-browser");
//import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
var forms_1 = require("@angular/forms");
var material_1 = require("@angular/material");
var animations_1 = require("@angular/platform-browser/animations");
var app_component_1 = require("./app.component");
var scores_tab_component_1 = require("./scores-tab/scores-tab.component");
var my_account_tab_component_1 = require("./my-account-tab/my-account-tab.component");
var registration_tab_component_1 = require("./registration-tab/registration-tab.component");
var offer_tab_component_1 = require("./offer-tab/offer-tab.component");
var contact_tab_component_1 = require("./contact-tab/contact-tab.component");
var about_us_tab_component_1 = require("./about-us-tab/about-us-tab.component");
var app_routing_module_1 = require("./app-routing/app-routing.module");
var tab_router_component_1 = require("./tab-router/tab-router.component");
var AppModule = /** @class */ (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        core_1.NgModule({
            imports: [
                platform_browser_1.BrowserModule, app_routing_module_1.AppRoutingModule,
                //NgbModule.forRoot(),
                forms_1.FormsModule,
                animations_1.BrowserAnimationsModule, material_1.MatButtonModule, material_1.MatTabsModule //Material
            ],
            declarations: [app_component_1.AppComponent, scores_tab_component_1.ScoresTabComponent, my_account_tab_component_1.MyAccountTabComponent, registration_tab_component_1.RegistrationTabComponent, offer_tab_component_1.OfferTabComponent, contact_tab_component_1.ContactTabComponent, about_us_tab_component_1.AboutUsTabComponent, tab_router_component_1.TabRouterComponent],
            bootstrap: [app_component_1.AppComponent]
        })
    ], AppModule);
    return AppModule;
}());
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map