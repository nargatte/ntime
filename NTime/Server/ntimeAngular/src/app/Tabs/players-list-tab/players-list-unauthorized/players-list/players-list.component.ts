import {
  Component,
  OnInit,
  AfterViewInit} from '@angular/core';
import { Observable } from 'rxjs';

import { PlayerService } from '../../../../Services/player.service';
import { MessageService } from '../../../../Services/message.service';
import { PageViewModel } from '../../../../Models/PageViewModel';
import { PlayerListView } from '../../../../Models/Players/PlayerListView';
import { PlayerFilterOptions } from '../../../../Models/Players/PlayerFilterOptions';
import { ActivatedRoute } from '@angular/router';
import { PlayersBaseListComponent } from '../../players-base-list.component';
import { ExtraColumn } from '../../../../Models/ExtraColumns/ExtraColumn';
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
export class PlayersListComponent extends PlayersBaseListComponent<PlayerListView> implements AfterViewInit, OnInit {

  constructor(
    protected playerService: PlayerService,
    protected messageService: MessageService,
    protected route: ActivatedRoute
  ) {
    super(playerService, messageService, route);
  }

  protected getPlayersFromService(
    competitionId: number,
    playerFilterOptions: PlayerFilterOptions,
    pageSize: number,
    pageNumber: number): Observable<PageViewModel<PlayerListView> | {}> {
      return this.playerService.getPlayerListView(competitionId, playerFilterOptions, pageSize, pageNumber);
  }

  protected filterExtraColumns(extraColumns: ExtraColumn[]): ExtraColumn[] {
    return extraColumns.filter(
      column => column.IsDisplayedInPublicList
    );
  }

  // getPlayers(competitionId: number, playerFilterOptions: PlayerFilterOptions,
  //   pageSize: number, pageNumber: number
  // ): void {
  //   this.dataLoaded = false;
  //   this.playerService
  //     .getPlayerListView(competitionId, playerFilterOptions, pageSize, pageNumber
  //     )
  //     .subscribe(
  //       (page: PageViewModel<PlayerListView>) => {
  //         this.log(`Items: ${page.TotalCount}`);
  //         this.players = page.Items;
  //         this.playersCount = page.TotalCount;
  //       },
  //       error => this.onError(error), // Errors
  //       () => this.setDataSource()
  //       // () => this.setDataSource() // Success
  //     );
  // }

  // onError(message: any) {
  //   this.dataLoaded = true;
  //   this.messageService.addError(message);
  // }

  // getFilteredPlayers(): void {
  //   this.dataLoaded = false;
  //   this.getPlayers(this.competitionId, this.filter, this.defaultPageSize, this.defaultPageNumber
  //   );
  // }

  // log(message: string): void {
  //   this.messageService.addLog(message);
  // }

  // setDataSource() {
  //   this.dataSource = new MatTableDataSource<PlayerListView>(this.players);
  //   this.dataLoaded = true;
  //   // this.table.renderRows();
  // }

  // onPageEvent(event: PageEvent) {
  //   this.defaultPageSize = event.pageSize;
  //   this.defaultPageNumber = event.pageIndex;
  //   this.getFilteredPlayers();
  // }

  // onSortEvent(event: Sort) {
  //   this.messageService.addLog('Requested sorting');
  //   this.messageService.addObject(event);
  //   const sortOrder = SortHelper.isSortDescending(event.direction);
  //   if (sortOrder === null) {
  //     this.setDefaultSorting();
  //   } else {
  //     this.filter.DescendingSort = sortOrder;
  //     this.filter.PlayerSort = SortHelper.getPlayerSort(event.active);
  //     if (this.filter.PlayerSort === PlayerSort.ByExtraData) {
  //       this.filter.ExtraDataSortIndex = Number.parseInt(event.active);
  //     }
  //   }

  //   this.getFilteredPlayers();
  // }

  // setDefaultSorting() {
  //   this.filter.PlayerSort = PlayerSort.ByRegisterDate;
  //   this.filter.DescendingSort = false;
  // }

  // prepareExtraColumns(): void {
  //   this.competition.ExtraColumns.filter(
  //     column => column.IsDisplayedInPublicList
  //   )
  //     .map(column => column.Id)
  //     .forEach(id => this.displayedColumns.push(id.toString()));
  // }



  // public textFilterChanged(term: string): void {
  //   debounceTime(300);
  //   distinctUntilChanged();
  //   this.filter.Query = term;
  //   this.filtersChanged();
  // }

  // public distanceFilterChanged(distance: Distance) {
  //   this.filter.Distance = distance;
  //   this.filtersChanged();
  // }

  // public subCategoryFilterChanged(subCategory: Subcategory) {
  //   this.filter.Subcategory = subCategory;
  //   this.filtersChanged();
  // }

  // public ageCategoryFilterChanged(ageCategory: AgeCategory) {
  //   this.filter.AgeCategory = ageCategory;
  //   this.filtersChanged();
  // }

  // public isPaidUpFilterChanged(isPaidUp: boolean) {
  //   this.filter.IsPaidUp = isPaidUp;
  //   this.filtersChanged();
  // }

  // public isMaleFilterChanged(isMale: boolean) {
  //   this.filter.IsMale = isMale;
  //   this.filtersChanged();
  // }

  // private filtersChanged() {
  //   this.defaultPageNumber = 0;
  //   this.getFilteredPlayers();
  // }

  // private setPagetOptions() {
  //   const pageOptions = this.playerService.getPaginationInfo();
  //   this.defaultPageNumber = pageOptions.defaultPageNumber;
  //   this.defaultPageSize = pageOptions.defaultPageSize;
  //   this.pageSizeOptions = pageOptions.pageSizeOptions;
  // }
}
