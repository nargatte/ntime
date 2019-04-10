import {
  Component,
  OnInit,
  ViewChild,
  AfterViewInit,
  Input} from '@angular/core';
import {
  MatPaginator,
  MatTableDataSource,
  MatTable,
  PageEvent,
  Sort
} from '@angular/material';
import { Subject } from 'rxjs';

import { PlayerService } from '../../../../Services/player.service';
import { MessageService } from '../../../../Services/message.service';
import { PageViewModel } from '../../../../Models/PageViewModel';
import { PlayerListView } from '../../../../Models/Players/PlayerListView';
import { PlayerFilterOptions } from '../../../../Models/Players/PlayerFilterOptions';
import { ActivatedRoute } from '@angular/router';
import { PlayerSort } from '../../../../Models/Enums/PlayerSort';
import { SortHelper } from '../../../../Helpers/SortHelper';
import { ExtraDataDefinition } from '../../../../Models/CDK/ExtraDataDefinition';
import {
  debounceTime,
  distinctUntilChanged} from 'rxjs/operators';
import { CompetitionWithDetails } from '../../../../Models/Competitions/CompetitionWithDetails';
// import { CompetitionWithDetails } from '../../../Models/Competitions/CompetitionWithDetails';

@Component({
  selector: 'app-players-list',
  templateUrl: './players-list.component.html',
  styleUrls: [
    './players-list.component.css',
    '../../../../app.component.css',
    '../../../../Styles/mobile-style.css'
  ]
})
export class PlayersListComponent implements AfterViewInit, OnInit {
  @Input() competition: CompetitionWithDetails;
  public todayDate: Date;
  public dataLoaded = false;
  private competitionId: number;
  private players: PlayerListView[] = [];
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
  oldExtraColumns: ExtraDataDefinition[] = [];
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
    // this.setDataSource();
    // this.competition = new Competition();
    this.todayDate = new Date(Date.now());
    this.competitionId = +this.route.snapshot.paramMap.get('id');
  }

  ngOnInit(): void {
    this.setDefaultSorting();
    this.getFilteredPlayers();
    this.prepareExtraColumns();
  }

  ngAfterViewInit() {}

  getPlayers(competitionId: number, playerFilterOptions: PlayerFilterOptions,
    pageSize: number, pageNumber: number
  ): void {
    this.dataLoaded = false;
    this.playerService
      .getPlayerListView(competitionId, playerFilterOptions, pageSize, pageNumber
      )
      .subscribe(
        (page: PageViewModel<PlayerListView>) => {
          this.log(`Items: ${page.TotalCount}`);
          this.players = page.Items;
          this.playersCount = page.TotalCount;
        },
        error => this.onError(error), // Errors
        () => this.setDataSource()
        // () => this.setDataSource() // Success
      );
  }

  onError(message: any) {
    this.dataLoaded = true;
    this.messageService.addError(message);
  }

  getFilteredPlayers(): void {
    this.dataLoaded = false;
    this.getPlayers(this.competitionId, this.filter, this.pageSize, this.pageNumber
    );
  }

  log(message: string): void {
    this.messageService.addLog(message);
  }

  setDataSource() {
    this.dataSource = new MatTableDataSource<PlayerListView>(this.players);
    this.dataLoaded = true;
    // this.table.renderRows();
  }

  onPageEvent(event: PageEvent) {
    this.pageSize = event.pageSize;
    this.pageNumber = event.pageIndex;
    this.getFilteredPlayers();
  }

  onSortEvent(event: Sort) {
    this.messageService.addLog('Requested sorting');
    this.messageService.addObject(event);
    const sortOrder = SortHelper.isSortDescending(event.direction);
    if (sortOrder === null) {
      this.setDefaultSorting();
    } else {
      this.filter.DescendingSort = sortOrder;
      this.filter.PlayerSort = SortHelper.getPlayerSort(event.active);
      if (this.filter.PlayerSort === PlayerSort.ByExtraData) {
        this.filter.ExtraDataSortIndex = Number.parseInt(event.active);
      }
    }

    this.getFilteredPlayers();
  }

  setDefaultSorting() {
    this.filter.PlayerSort = PlayerSort.ByRegisterDate;
    this.filter.DescendingSort = false;
  }

  prepareExtraColumns(): void {
    this.competition.ExtraColumns.filter(
      column => column.IsDisplayedInPublicList
    )
      .map(column => column.Id)
      .forEach(id => this.displayedColumns.push(id.toString()));
  }

  search(term: string): void {
    this.pageNumber = 0;
    debounceTime(300);
    distinctUntilChanged();
    this.filter.Query = term;
    this.getFilteredPlayers();
  }
}
