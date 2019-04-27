import {
  Component,
  OnInit,
  ViewChild,
  AfterViewInit,
  Input
} from '@angular/core';
import {
  MatPaginator,
  MatTableDataSource,
  MatTable,
  PageEvent,
  Sort
} from '@angular/material';

import {
  debounceTime,
  distinctUntilChanged
} from 'rxjs/operators';
import { CompetitionWithDetails } from '../../Models/Competitions/CompetitionWithDetails';
import { Distance } from '../../Models/Distance';
import { Subcategory } from '../../Models/Subcategory';
import { AgeCategory } from '../../Models/AgeCategory';
import { PlayerListView } from '../../Models/Players/PlayerListView';
import { PlayerFilterOptions } from '../../Models/Players/PlayerFilterOptions';
import { PlayerService } from '../../Services/player.service';
import { MessageService } from '../../Services/message.service';
import { ActivatedRoute } from '@angular/router';
import { PageViewModel } from '../../Models/PageViewModel';
import { SortHelper } from '../../Helpers/SortHelper';
import { PlayerSort } from '../../Models/Enums/PlayerSort';
import { ExtraColumn } from '../../Models/ExtraColumns/ExtraColumn';
import { Observable } from 'rxjs';

export abstract class PlayersBaseListComponent<TPlayer extends PlayerListView> implements AfterViewInit, OnInit {
  @Input() competition: CompetitionWithDetails;
  public todayDate: Date;
  public dataLoaded = false;
  protected competitionId: number;
  protected players: TPlayer[] = [];
  protected filter: PlayerFilterOptions = new PlayerFilterOptions();

  @ViewChild(MatTable) table: MatTable<TPlayer>;
  displayedColumns = [
    'firstName',
    'lastName',
    'city',
    'team',
    'fullCategory',
    'isPaidUp'
  ];
  dataSource: MatTableDataSource<TPlayer>;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  public defaultPageNumber: number;
  public defaultPageSize: number;
  public pageSizeOptions: number[];
  public playersCount = 0;

  constructor(
    protected playerService: PlayerService,
    protected messageService: MessageService,
    protected route: ActivatedRoute
  ) {
    // this.setDataSource();
    // this.competition = new Competition();
    this.todayDate = new Date(Date.now());
    this.competitionId = +this.route.snapshot.paramMap.get('id');
    this.setPagetOptions();
  }

  ngOnInit(): void {
    this.setDefaultSorting();
    this.getFilteredPlayers();
    this.prepareExtraColumns();
  }

  ngAfterViewInit() { }

  getPlayers(competitionId: number, playerFilterOptions: PlayerFilterOptions,
    pageSize: number, pageNumber: number
  ): void {
    this.dataLoaded = false;
    this.getPlayersFromService(competitionId, playerFilterOptions, pageSize, pageNumber
    )
      .subscribe(
        (page: PageViewModel<TPlayer>) => {
          this.log(`Items: ${page.TotalCount}`);
          this.players = page.Items;
          this.playersCount = page.TotalCount;
        },
        error => this.onError(error), // Errors
        () => this.setDataSource()
        // () => this.setDataSource() // Success
      );
  }

  protected abstract getPlayersFromService(
    competitionId: number,
    playerFilterOptions: PlayerFilterOptions,
    pageSize: number,
    pageNumber: number): Observable<PageViewModel<TPlayer> | {}>;

  onError(message: any) {
    this.dataLoaded = true;
    this.messageService.addError(message);
  }

  getFilteredPlayers(): void {
    this.dataLoaded = false;
    this.getPlayers(this.competitionId, this.filter, this.defaultPageSize, this.defaultPageNumber
    );
  }

  log(message: string): void {
    this.messageService.addLog(message);
  }

  setDataSource() {
    this.dataSource = new MatTableDataSource<TPlayer>(this.players);
    this.dataLoaded = true;
    // this.table.renderRows();
  }

  onPageEvent(event: PageEvent) {
    this.defaultPageSize = event.pageSize;
    this.defaultPageNumber = event.pageIndex;
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
    this.filterExtraColumns(this.competition.ExtraColumns)
      .map(column => column.Id)
      .forEach(id => this.displayedColumns.push(id.toString()));
  }

  protected abstract filterExtraColumns(extraColumns: ExtraColumn[]): ExtraColumn[];

  public textFilterChanged(term: string): void {
    debounceTime(300);
    distinctUntilChanged();
    this.filter.Query = term;
    this.filtersChanged();
  }

  public distanceFilterChanged(distance: Distance) {
    this.filter.Distance = distance;
    this.filtersChanged();
  }

  public subCategoryFilterChanged(subCategory: Subcategory) {
    this.filter.Subcategory = subCategory;
    this.filtersChanged();
  }

  public ageCategoryFilterChanged(ageCategory: AgeCategory) {
    this.filter.AgeCategory = ageCategory;
    this.filtersChanged();
  }

  public isPaidUpFilterChanged(isPaidUp: boolean) {
    this.filter.IsPaidUp = isPaidUp;
    this.filtersChanged();
  }

  public isMaleFilterChanged(isMale: boolean) {
    this.filter.IsMale = isMale;
    this.filtersChanged();
  }

  protected filtersChanged() {
    this.defaultPageNumber = 0;
    this.getFilteredPlayers();
  }

  protected setPagetOptions() {
    const pageOptions = this.playerService.getPaginationInfo();
    this.defaultPageNumber = pageOptions.defaultPageNumber;
    this.defaultPageSize = pageOptions.defaultPageSize;
    this.pageSizeOptions = pageOptions.pageSizeOptions;
  }
}
