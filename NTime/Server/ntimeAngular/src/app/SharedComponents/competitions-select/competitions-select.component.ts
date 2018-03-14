import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, Validators, ReactiveFormsModule } from '@angular/forms'
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MatPaginator, MatTableDataSource, MatTable } from '@angular/material';

import { Competition } from '../../Models/Competition';
import { CompetitionService } from '../../Services/competition.service'


@Component({
    selector: 'app-competitions-select',
    templateUrl: './competitions-select.component.html',
    styleUrls: ['./competitions-select.component.css']
})
export class CompetitionsSelectComponent implements OnInit {
    competitions: Competition[] = [];

    constructor(private competitionService: CompetitionService) { }

    ngOnInit() {
        // this.getHeroes();
    }

    displayedColumns = ['name', 'city', 'date', 'signUpEndDate', 'link'];
    dataSource: MatTableDataSource<Competition> = new MatTableDataSource<Competition>(this.competitions);

    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatTable) table: MatTable<Competition>;

    /**
     * Set the paginator after the view init since this component will
     * be able to query its view for the initialized paginator.
     */
    ngAfterViewInit() {
        this.getCompetitions();
        //    this.dataSource.paginator = this.paginator;

    }


    getCompetitions(): void {
        this.competitionService.getCompetitions().subscribe(
            competitions => {
                this.pushCompetitions(competitions);
                this.renderTable();
            }
        );
        // this.renderTable();
    }
    // getCompetitions(): void {
    //     this.pushCompetitions(this.competitionService.getCompetitions());
    //     this.renderTable();
    // }

    pushCompetitions(competitions): void {
        competitions.forEach(element => {
            this.competitions.push(element);
        });
    }

    renderTable(): void {
        // this.dataSource = new MatTableDataSource<Competition>(this.competitions);
        // this.dataSource.paginator = this.paginator;
        this.dataSource.paginator = this.paginator;
        this.table.renderRows();
    }
}
