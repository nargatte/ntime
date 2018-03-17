import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, Validators, ReactiveFormsModule } from '@angular/forms'
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MatPaginator, MatTableDataSource, MatTable } from '@angular/material';
import { DataSource } from '@angular/cdk/table';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

import { Competition } from '../../Models/Competition';
import { CompetitionService } from '../../Services/competition.service'
import { PageViewModel } from '../../Models/PageViewModel';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/merge';
import 'rxjs/add/operator/map';


@Component({
    selector: 'app-competitions-select',
    templateUrl: './competitions-select.component.html',
    styleUrls: ['./competitions-select.component.css']
})
export class CompetitionsSelectComponent {
    competitions: Competition[] = [];
    public todayDate: Date;

    constructor(private competitionService: CompetitionService) {
        this.todayDate = new Date(Date.now());
    }

    displayedColumns = ['name', 'city', 'date', 'signUpEndDate', 'link', 'actions'];
    dataSource: MatTableDataSource<Competition> = new MatTableDataSource<Competition>(this.competitions);

    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatTable) table: MatTable<Competition>;

    /**
     * Set the paginator after the view init since this component will
     * be able to query its view for the initialized paginator.
     */
    ngAfterViewInit() {
        this.setPaginator();
        this.getCompetitions();
    }

    getCompetitions(): void {
        this.competitionService.getCompetitions().subscribe(
            (page: PageViewModel<Competition>) => {
                console.log(page)
                console.log(`Items: ${page.TotalCount}`);
                this.competitions = page.Items;
                this.competitions = this.convertDates(this.competitions);
            },
            error => console.log(error), //Errors
            () => this.setDataSource() //Success
        );
    }

    private convertDates(competitions: Competition[]) {
        competitions.forEach(element => {
            element.EventDate = new Date(element.EventDate);
            element.SignUpEndDate = new Date(element.SignUpEndDate);
        });
        return competitions;
    }

    setPaginator(): void {
        this.dataSource.paginator = this.paginator;
    }
    setDataSource() {
        this.dataSource = new MatTableDataSource<Competition>(this.competitions);
        console.log("Datasource set");
    }

}
