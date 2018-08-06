import {
  Component,
  OnInit,
  ViewChild,
  AfterViewInit,
  Input,
  EventEmitter
} from '@angular/core';
import { FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {
  MatPaginator,
  MatTableDataSource,
  MatTable,
  PageEvent,
  MatSort,
  Sort
} from '@angular/material';
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
import { PlayerSort } from '../../../Models/Enums/PlayerSort';
import { SortHelper } from '../../../Helpers/SortHelper';

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
  displayedColumns = [
    'firstName',
    'lastName',
    'city',
    'team',
    'fullCategory',
    'isPaidUp'
  ];
  columns = [
    { columnDef: 'userId', header: 'ID', cell: (row: UserData) => `${row.id}` },
    {
      columnDef: 'userName',
      header: 'Name',
      cell: (row: UserData) => `${row.name}`
    },
    {
      columnDef: 'progress',
      header: 'Progress',
      cell: (row: UserData) => `${row.progress}%`
    }
  ];
  dataSource: MatTableDataSource<PlayerListView>;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  public pageNumber = 0;
  public pageSize = 50;
  public pageSizeOptions = [20, 50, 100];
  public playersCount = 0;

  constructor(
    private playerService: PlayerService,
    private messageService: MessageService,
    private route: ActivatedRoute
  ) {
    this.todayDate = new Date(Date.now());
    this.competitionId = +this.route.snapshot.paramMap.get('id');
    this.columns.map(x => x.columnDef).forEach(c => this.displayedColumns.push(c));
  }

  ngOnInit(): void {}

  ngAfterViewInit() {
    this.setDefaultSorting();
    this.getFilteredPlayers();
  }

  getPlayers(
    competitionId: number,
    playerFilterOptions: PlayerFilterOptions,
    pageSize: number,
    pageNumber: number
  ): void {
    this.playerService
      .getPlayerListView(
        competitionId,
        playerFilterOptions,
        pageSize,
        pageNumber
      )
      .subscribe(
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

  getFilteredPlayers(): void {
    this.getPlayers(
      this.competitionId,
      this.filter,
      this.pageSize,
      this.pageNumber
    );
  }

  log(message: string): void {
    this.messageService.addLog(message);
  }

  setDataSource() {
    this.dataSource = new MatTableDataSource<PlayerListView>(this.players);
    // this.table.renderRows();
  }

  onPageEvent(event: PageEvent) {
    this.pageSize = event.pageSize;
    this.pageNumber = event.pageIndex;
    this.getFilteredPlayers();
  }

  onSortEvent(event: Sort) {
    const sortOrder = SortHelper.isSortDescending(event.direction);
    if (sortOrder === null) {
      this.setDefaultSorting();
    } else {
      this.filter.DescendingSort = sortOrder;
      this.filter.PlayerSort = SortHelper.getPlayerSort(event.active);
    }

    this.getFilteredPlayers();
  }

  setDefaultSorting() {
    this.filter.PlayerSort = PlayerSort.ByLastName;
    this.filter.DescendingSort = false;
  }
}

export interface UserData {
  id: number;
  name: string;
  progress: number;
}
