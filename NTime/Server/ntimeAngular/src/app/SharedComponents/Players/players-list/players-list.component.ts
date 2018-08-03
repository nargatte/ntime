import { Component, OnInit, ViewChild, AfterViewInit, Input, EventEmitter } from '@angular/core';
import { FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MatPaginator, MatTableDataSource, MatTable, PageEvent, MatSort, Sort } from '@angular/material';
import { DataSource } from '@angular/cdk/table';
import { BehaviorSubject, Observable } from 'rxjs';



import { PlayerService } from '../../../Services/player.service';
import { MessageService } from '../../../Services/message.service';
import { PageViewModel } from '../../../Models/PageViewModel';
import { PlayerListView } from '../../../Models/Players/PlayerListView';
import { Competition } from '../../../Models/Competition';
import { PlayerFilterOptions } from '../../../Models/Players/PlayerFilterOptions';
import { ActivatedRoute } from '@angular/router';
import { MockPlayersListView } from '../../../MockData/MockPlayers';

@Component({
  selector: 'app-players-list',
  templateUrl: './players-list.component.html',
  styleUrls: ['./players-list.component.css']
})
export class PlayersListComponent implements AfterViewInit, OnInit {

  @Input() competition: Competition;
  private competitionId: number;
  private players: PlayerListView[] = [];
  public todayDate: Date;
  private filter: PlayerFilterOptions = new PlayerFilterOptions();

  @ViewChild(MatTable) table: MatTable<PlayerListView>;
  displayedColumns = ['firstName', 'lastName', 'city', 'team', 'fullCategory', 'isPaidUp'];
  dataSource: MatTableDataSource<PlayerListView>;
  @ViewChild(MatSort) sort: MatSort;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  public pageNumber = 0;
  public pageSize = 50;
  public pageSizeOptions = [20, 50, 100];
  public playersCount = 0;

  constructor(
    private playerService: PlayerService,
    private messageService: MessageService,
    private route: ActivatedRoute) {
    this.todayDate = new Date(Date.now());
    this.competitionId = +this.route.snapshot.paramMap.get('id');
  }

  ngOnInit(): void {
    // this.dataSource.sort = this.sort;
  }

  ngAfterViewInit() {
    this.getPlayers(this.competitionId, this.filter, this.pageSize, this.pageNumber);
    // this.setDataSource();
  }

  getPlayers(competitionId: number, playerFilterOptions: PlayerFilterOptions, pageSize: number, pageNumber: number): void {
    // this.players = MockPlayersListView;
    // this.playersCount = 3;
    // this.dataSource = new MatTableDataSource<PlayerListView>(this.players);
    // return;
    this.playerService.getPlayerListView(competitionId, playerFilterOptions, pageSize, pageNumber).subscribe(
      (page: PageViewModel<PlayerListView>) => {
        this.log(`Items: ${page.TotalCount}`);
        this.players = page.Items;
        this.playersCount = page.TotalCount;
      },
      error => this.log(error), // Errors
      () => this.setDataSource()
      // () => this.setDataSource() // Success
    );
  }

  log(message: string): void {
    this.messageService.addLog(message);
  }

  setDataSource() {
    this.dataSource = new MatTableDataSource<PlayerListView>(this.players);
    this.dataSource.sort = this.sort;
    // this.table.renderRows();
  }

  onPageEvent(event: PageEvent) {
    this.getPlayers(this.competition.Id, this.filter, event.pageSize, event.pageIndex);
  }

  onSortEvent(event: Sort) {
    console.log(event.active);
    console.log(event.direction);
    // this.dataSource.sort.sortChange.forEach(p=> p.direction)
    // this.getPlayers(this.competition.Id, this.filter, event.pageSize, event.pageIndex);
  }
}
