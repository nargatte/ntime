import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MatPaginator, MatTableDataSource, MatTable, PageEvent } from '@angular/material';
import { DataSource } from '@angular/cdk/table';
import { BehaviorSubject, Observable } from 'rxjs';
import { PageViewModel } from '../../../Models/PageViewModel';



import { Competition } from '../../../Models/Competition';
import { CompetitionService } from '../../../Services/competition.service';
import { MessageService } from '../../../Services/message.service';
import { AuthenticatedUserService } from '../../../Services/authenticated-user.service';
import { RoleEnum } from '../../../Models/Enums/RoleEnum';
import { OrganizerAccountService } from '../../../Services/organizer-account.service';
import { OrganizerAccount } from '../../../Models/OrganizerAccount';


@Component({
    selector: 'app-competitions-select',
    templateUrl: './competitions-select.component.html',
    styleUrls: ['./competitions-select.component.css']
})
export class CompetitionsSelectComponent implements AfterViewInit {
    public todayDate: Date;
    public isRegistrationAvailable = true;

    constructor(
        private competitionService: CompetitionService,
        private messageService: MessageService,
        private authenticatedUserService: AuthenticatedUserService,
        private organizerAccountService: OrganizerAccountService) {
        if (this.authenticatedUserService.IsAuthenticated === true && this.authenticatedUserService.User.Role !== RoleEnum.Player) {
            this.isRegistrationAvailable = false;
        }
        this.todayDate = new Date(Date.now());
    }

    @ViewChild(MatTable) table: MatTable<Competition>;
    competitions: Competition[] = [];
    displayedColumns = ['name', 'city', 'date', 'signUpEndDate', 'link', 'actions'];
    dataSource: MatTableDataSource<Competition> = new MatTableDataSource<Competition>(this.competitions);

    @ViewChild(MatPaginator) paginator: MatPaginator;
    public pageNumber = 0;
    public pageSize = 20;
    public pageSizeOptions = [10, 20, 50];
    public competitionsCount = 0;

    /**
     * Set the paginator after the view init since this component will
     * be able to query its view for the initialized paginator.
     */
    ngAfterViewInit() {
        this.getCompetitions(this.pageSize, this.pageNumber);
    }

    getCompetitions(pageSize: number, pageNumber: number): void {
        if (this.authenticatedUserService.IsAuthenticated === true && this.authenticatedUserService.User.Role === RoleEnum.Organizer) {
            this.organizerAccountService.getMyInfo().subscribe(
                (organizer: OrganizerAccount) => {
                    this.messageService.addLog(`Items: ${organizer.CompetitionDtos.length}`);
                    this.competitionsCount = organizer.CompetitionDtos.length;
                    this.competitions = organizer.CompetitionDtos;
                    this.competitions.forEach(competition => Competition.convertDates(competition));
                },
                error => this.messageService.addError(error), // Errors
                () => this.setDataSource() // Success
            );
        } else {
            this.competitionService.getCompetitions(pageSize, pageNumber).subscribe(
                (page: PageViewModel<Competition>) => {
                    this.messageService.addLog(`Items: ${page.TotalCount}`);
                    this.competitionsCount = page.TotalCount;
                    this.competitions = page.Items;
                    this.competitions.forEach(competition => Competition.convertDates(competition));
                },
                error => this.messageService.addError(error), // Errors
                () => this.setDataSource() // Success
            );
        }
    }

    private log(message: string) {
        this.messageService.addLog(message);
    }

    private setDataSource() {
        this.dataSource = new MatTableDataSource<Competition>(this.competitions);
        this.messageService.addLog('Datasource set');
    }

    onPageEvent(event: PageEvent) {
        this.getCompetitions(event.pageSize, event.pageIndex);
    }
}
