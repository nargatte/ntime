import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, Validators, ReactiveFormsModule } from '@angular/forms'
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MatPaginator, MatTableDataSource, MatTable } from '@angular/material';
import {DataSource} from '@angular/cdk/table';
import {BehaviorSubject} from 'rxjs/BehaviorSubject';

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
export class CompetitionsSelectComponent implements OnInit {
    competitions: Competition[] = [
        new Competition(3, "Szczecin", new Date(2018, 9, 13,), new Date(2018, 9,11)),
    ];

    constructor(private competitionService: CompetitionService) { }

    displayedColumns = ['name', 'city', 'date', 'signUpEndDate', 'link'];
    dataSource: MatTableDataSource<Competition> = new MatTableDataSource<Competition>(this.competitions);
    
    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatTable) table: MatTable<Competition>;

    ngOnInit() {
        // this.dataSource = new ExampleDataSource(, this.paginator)
        // this.render();
    }
    
    
    /**
     * Set the paginator after the view init since this component will
     * be able to query its view for the initialized paginator.
     */
    ngAfterViewInit() {
        this.renderTable();
        this.getCompetitions();
    }


    getCompetitions(): void {
        this.competitionService.getCompetitions().subscribe(
            (page : PageViewModel<Competition>) => {
                console.log(page)
                console.log(`Items: ${page.TotalCount}`);
                this.competitions = page.Items;
                this.convertDates();
                // page.Items.forEach(competition => {
                //     this.competitions.push(competition);
                // });
            },
            error => console.log(error) , //Errors
            // () => this.table.renderRows()
            this.render //Success
        );
    }
    
    private convertDates() {
        this.competitions.forEach(element => {
            let date = new Date(element.EventDate);
            element.EventDate = date;
            date = new Date(element.SignUpEndDate);
            element.SignUpEndDate = date;
        });
    }

    onSuccessfullLoading(){
        // this.dataSource = new MatTableDataSource<Competition>(this.competitions)
        // this.table.renderRows();
        console.log('Competitions fetched successfully');
        // this.dataSource = new MatTableDataSource<Competition>(this.competitions);        
        // this.render();
    }

    renderTable(): void {
        this.dataSource.paginator = this.paginator;
        this.table.renderRows();
    }
    render(){
        // this.competitions.push(new Competition(3, "Szczecin2", new Date(2018, 9, 13)))
        this.dataSource = new MatTableDataSource<Competition>(this.competitions);
        console.log("Button clicked");
        // this.dataSource.data = this.competitions;
        this.table.renderRows();
    }

}

// export class ExampleDataSource extends DataSource<any> {
//     _filterChange = new BehaviorSubject('');
//     get filter(): string { return this._filterChange.value; }
//     set filter(filter: string) { this._filterChange.next(filter); }
  
//     filteredData: Competition[] = [];
//     renderedData: Competition[] = [];
  
//     constructor(private _exampleDatabase: ExampleDatabase,
//                 private _paginator: MatPaginator) {
//       super();
      
//       this._filterChange.subscribe(() => this._paginator.pageIndex = 0);
//     }
  
//     /** Connect function called by the table to retrieve one stream containing the data to render. */
//     connect(): Observable<Competition[]> {
//       // Listen for any changes in the base data, sorting, filtering, or pagination
//       const displayDataChanges = [
//         this._exampleDatabase.dataChange,
//         this._paginator.page,
//       ];
      

//     //   return Observable.merge(...displayDataChanges).map(() => {
//     //     // Filter data
//     //     this.filteredData = this._exampleDatabase.data.slice().filter((item: Competition) => {
//     //       let searchStr = (item.name + item.color).toLowerCase();
//     //       return searchStr.indexOf(this.filter.toLowerCase()) != -1;
//     //     });

  
//     //     // Grab the page's slice of the filtered sorted data.
//     //     const startIndex = this._paginator.pageIndex * this._paginator.pageSize;
//     //     this.renderedData = sortedData.splice(startIndex, this._paginator.pageSize);
//     //     return this.renderedData;
//     //   });
//     }
  
//     disconnect() {}
//   }