import { Component, OnInit } from '@angular/core';
import { FormControl, Validators, ReactiveFormsModule } from '@angular/forms'
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Competition } from '../../Models/Competition';
import { CompetitionService } from '../../Services/competition.service'


@Component({
    selector: 'app-competitions-select',
    templateUrl: './competitions-select.component.html',
    styleUrls: ['./competitions-select.component.css']
})
export class CompetitionsSelectComponent implements OnInit {
    competitions: Competition[];

    constructor(private competitionService: CompetitionService) { }

    ngOnInit() {
        this.getHeroes();
    }

    getHeroes(): void {
        this.competitionService.getCompetitions().subscribe(
            competitions => this.competitions = competitions
        );
    }


    //foods = [
    //    { value: 'steak-0', viewValue: 'Steak' },
    //    { value: 'pizza-1', viewValue: 'Pizza' },
    //    { value: 'tacos-2', viewValue: 'Tacos' }
    //];
}
