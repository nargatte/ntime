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
var http_1 = require("@angular/common/http");
var material_1 = require("@angular/material");
var animations_1 = require("@angular/platform-browser/animations");
//import { FlexLayoutModule } from '@angular/flex-layout';
var app_component_1 = require("./app.component");
var scores_tab_component_1 = require("./Tabs/scores-tab/scores-tab.component");
var my_account_tab_component_1 = require("./Tabs/my-account-tab/my-account-tab.component");
var registration_tab_component_1 = require("./Tabs/registration-tab/registration-tab.component");
var offer_tab_component_1 = require("./Tabs/offer-tab/offer-tab.component");
var contact_tab_component_1 = require("./Tabs/contact-tab/contact-tab.component");
var about_us_tab_component_1 = require("./Tabs/about-us-tab/about-us-tab.component");
var app_routing_module_1 = require("./app-routing/app-routing.module");
var navbar_component_1 = require("./navbar/navbar.component");
var competitions_select_component_1 = require("./SharedComponents/competitions-select/competitions-select.component");
var competition_service_1 = require("./Services/competition.service");
var message_service_1 = require("./Services/message.service");
var AppModule = /** @class */ (function () {
    function AppModule() {
    }
    AppModule = __decorate([
        core_1.NgModule({
            imports: [
                platform_browser_1.BrowserModule, forms_1.FormsModule, app_routing_module_1.AppRoutingModule, forms_1.ReactiveFormsModule, http_1.HttpClientModule,
                //NgbModule.forRoot(),
                animations_1.BrowserAnimationsModule, material_1.MatButtonModule, material_1.MatTabsModule, material_1.MatIconModule,
                material_1.MatToolbarModule //Material 
            ],
            declarations: [app_component_1.AppComponent, scores_tab_component_1.ScoresTabComponent, my_account_tab_component_1.MyAccountTabComponent,
                registration_tab_component_1.RegistrationTabComponent, offer_tab_component_1.OfferTabComponent, contact_tab_component_1.ContactTabComponent,
                about_us_tab_component_1.AboutUsTabComponent, navbar_component_1.NavbarComponent, competitions_select_component_1.CompetitionsSelectComponent],
            providers: [
                competition_service_1.CompetitionService, message_service_1.MessageService
            ],
            bootstrap: [app_component_1.AppComponent]
        })
    ], AppModule);
    return AppModule;
}());
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map