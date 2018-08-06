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
import { String, StringBuilder } from 'typescript-string-operations';

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
import { ExtraColumnDefinition } from '../../../Models/CDK/ExtraColumnDefinition';


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
  private delimiter = '|';

  @ViewChild(MatTable) table: MatTable<PlayerListView>;
  displayedColumns = ['firstName', 'lastName', 'city', 'team', 'fullCategory', 'isPaidUp'];
  extraColumns: ExtraColumnDefinition[] = [];
  dataSource: MatTableDataSource<PlayerListView>;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  public pageNumber = 0;
  public pageSize = 50;
  public pageSizeOptions = [20, 50, 100];
  public playersCount = 0;

  constructor(
    private playerService: PlayerService,
    private messageService: MessageService,
    private route: ActivatedRoute,
  ) {
    // this.setDataSource();
    this.todayDate = new Date(Date.now());
    this.competitionId = +this.route.snapshot.paramMap.get('id');
  }

  ngOnInit(): void {
    this.setDefaultSorting();
    this.getFilteredPlayers();
    this.prepareExtraColumns();
  }

  ngAfterViewInit() {
  }

  getPlayers(
    competitionId: number,
    playerFilterOptions: PlayerFilterOptions,
    pageSize: number,
    pageNumber: number
  ): void {
    this.playerService
      .getPlayerListView(competitionId, playerFilterOptions, pageSize, pageNumber
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

  prepareExtraColumns(): void {
    this.messageService.addObject(this.competition);
    this.messageService.addLog(`ExtraDataHeaders: ${this.competition.ExtraDataHeaders}`);
    if (String.IsNullOrWhiteSpace(this.competition.ExtraDataHeaders)) {
      return;
    }

    const splitColumns = this.competition.ExtraDataHeaders.split(this.delimiter);
    let iterator = 0;
    splitColumns.forEach(columnString => {
      this.extraColumns.push(
        new ExtraColumnDefinition(iterator.toString(), columnString, iterator, this.delimiter)
      );
      iterator++;
    });

    this.extraColumns
      .map(x => x.columnDef)
      .forEach(c => this.displayedColumns.push(c));
  }
}
