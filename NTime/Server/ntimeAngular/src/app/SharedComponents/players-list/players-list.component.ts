import { Component, OnInit, ViewChild, AfterViewInit, Input } from '@angular/core';
import { FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MatPaginator, MatTableDataSource, MatTable, PageEvent } from '@angular/material';
import { DataSource } from '@angular/cdk/table';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/merge';
import 'rxjs/add/operator/map';

import { PlayerService } from '../../Services/player.service';
import { MessageService } from '../../Services/message.service';
import { PageViewModel } from '../../Models/PageViewModel';
import { PlayerListView } from '../../Models/PlayerListView';
import { Competition } from '../../Models/Competition';
import { PlayerFilterOptions } from '../../Models/PlayerFilterOptions';

@Component({
  selector: 'app-players-list',
  templateUrl: './players-list.component.html',
  styleUrls: ['./players-list.component.css']
})
export class PlayersListComponent implements AfterViewInit {
  @Input() competition: Competition;
  private players: PlayerListView[] = [];
  public todayDate: Date;
  private filter: PlayerFilterOptions = new PlayerFilterOptions();

  @ViewChild(MatTable) table: MatTable<PlayerListView>;
  displayedColumns = ['firstName', 'lastName', 'sex', 'team', 'fullCategory'];
  dataSource: MatTableDataSource<PlayerListView> = new MatTableDataSource<PlayerListView>(this.players);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  public pageNumber = 0;
  public pageSize = 20;
  public pageSizeOptions = [10, 20, 50];
  public playersCount = 0;
  // public pageEvent: PageEvent;

  constructor(private playerService: PlayerService, private messageService: MessageService) {
    this.todayDate = new Date(Date.now());
  }

  ngAfterViewInit() {
    this.getPlayers(this.competition.Id, this.filter, this.pageSize, this.pageNumber);
    // this.setPaginator();
  }

  getPlayers(competitionId: number, playerFilterOptions: PlayerFilterOptions, pageSize: number, pageNumber: number): void {
    this.playerService.getPlayerListView(competitionId, playerFilterOptions, pageSize, pageNumber).subscribe(
      (page: PageViewModel<PlayerListView>) => {
        console.log(page);
        console.log(`Items: ${page.TotalCount}`);
        this.players = page.Items;
        this.playersCount = page.TotalCount;
      },
      error => console.log(error), // Errors
      () => this.setDataSource() // Success
    );
  }

  log(message: string): void {
    this.messageService.addLog(message);
  }

  setDataSource() {
    this.dataSource = new MatTableDataSource<PlayerListView>(this.players);
    this.log('Datasource set');
    // this.setPaginator();
  }

  setPaginator(): void {
    // this.dataSource.paginator = this.paginator;
    this.playersCount = 100;
  }

  onPageEvent(event: PageEvent) {
    this.getPlayers(this.competition.Id, this.filter, event.pageSize, event.pageIndex);
  }

  onClick(): void {
    this.dataSource.paginator = this.paginator;
  }
}
