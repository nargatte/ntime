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
  MatDialog,
  Sort
} from '@angular/material';
import { Observable } from 'rxjs';

import { PlayerService } from '../../../../Services/player.service';
import { MessageService } from '../../../../Services/message.service';
import { PageViewModel } from '../../../../Models/PageViewModel';
import { PlayerListView } from '../../../../Models/Players/PlayerListView';
import { Competition } from '../../../../Models/Competitions/Competition';
import { PlayerFilterOptions } from '../../../../Models/Players/PlayerFilterOptions';
import { ActivatedRoute } from '@angular/router';
import { SelectionModel } from '@angular/cdk/collections';
// tslint:disable-next-line:max-line-length
import { SuccessfullActionDialogComponent } from '../../../../SharedComponents/Dialogs/successfull-action-dialog/successfull-action-dialog.component';
import { FailedActionDialogComponent } from '../../../../SharedComponents/Dialogs/failed-action-dialog/failed-action-dialog.component';
import { PlayersWithScores } from '../../../../Models/Players/PlayerWithScores';
import { PlayerSort } from '../../../../Models/Enums/PlayerSort';
import { SortHelper } from '../../../../Helpers/SortHelper';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { ConfirmActionDialogComponent } from '../../../../SharedComponents/Dialogs/confirm-action-dialog/confirm-action-dialog.component';
import { PlayersBaseListComponent } from '../../players-base-list.component';
import { templateJitUrl } from '@angular/compiler';
import { ExtraColumn } from '../../../../Models/ExtraColumns/ExtraColumn';

@Component({
  selector: 'app-players-select',
  templateUrl: './players-select.component.html',
  styleUrls: [
    './players-select.component.css',
    '../../../../app.component.css',
    '../../../../Styles/mobile-style.css'
  ]
})
export class PlayersSelectComponent extends PlayersBaseListComponent<PlayersWithScores> implements OnInit, AfterViewInit {
  selection = new SelectionModel<PlayersWithScores>(true, []);

  constructor(
    protected playerService: PlayerService,
    protected messageService: MessageService,
    protected route: ActivatedRoute,
    protected dialog: MatDialog
  ) {
    super(playerService, messageService, route);
    // this.addStaffColumns();
  }

  protected getPlayersFromService(
    competitionId: number,
    playerFilterOptions: PlayerFilterOptions,
    pageSize: number,
    pageNumber: number): Observable<PageViewModel<PlayersWithScores> | {}> {
      return this.playerService.getPlayersWithScores(competitionId, playerFilterOptions, pageSize, pageNumber);
  }

  protected filterExtraColumns(extraColumns: ExtraColumn[]): ExtraColumn[] {
    return extraColumns;
  }

  protected extraOnInitMethods() {
    this.addStaffColumns();
    }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected()
      ? this.selection.clear()
      : this.dataSource.data.forEach(row => this.selection.select(row));
  }


  private addStaffColumns(): void {
    this.displayedColumns.unshift('select');
    this.displayedColumns.push('editPlayer');
    this.displayedColumns.push('deletePlayer');
  }


  public paidButtonClick(): void {
    this.saveSelectedPlayersPaid(true);
  }

  public notPaidButtonClick(): void {
    this.saveSelectedPlayersPaid(false);
  }

  private saveSelectedPlayersPaid(isPaid: boolean) {
    this.dataLoaded = false;
    const selectedPlayers = this.selection.selected;
    selectedPlayers.forEach(player => {
      player.IsPaidUp = isPaid;
    });
    this.playerService.updateMulitplePlayers(selectedPlayers).subscribe(
      () => {
        this.selection.clear();
        this.getPlayers(
          this.competition.Id,
          this.filter,
          this.defaultPageSize,
          this.defaultPageNumber
        );
        this.onSuccessDialog('Zmiany zostały zapisane');
      },
      error => {
        this.messageService.addError(`Could not save changes`);
        this.messageService.addObject(error);
        this.onFailureDialog('Nie udało się zapisać zmian');
      }
    );
  }

  public onDeletePlayerClicked(playerToDelete: PlayersWithScores): void {
    this.onConfirmDialog(
      'Czy na pewno chcesz usunąć tego zawodnika? Zmiana jest nieodwracalna.'
    ).subscribe(dialogResult => {
      this.messageService.addLog(`Dialog result: ${dialogResult}`);
      if (dialogResult === true) {
        this.deletePlayer(playerToDelete);
      }
    });
  }

  public deletePlayer(playerToDelete: PlayersWithScores): void {
    this.messageService.addLog(
      `Player with id: ${playerToDelete.Id} to delete`
    );
    this.dataLoaded = false;
    this.playerService.deletePlayer(playerToDelete.Id).subscribe(
      () => {
        this.onSuccessDialog('Zawodnik został usunięty');
        this.getFilteredPlayers();
      },
      (error: Error) =>
        this.onFailureDialog(
          `Nie udało się usunąć zawodnika: ${error.message}`
        ),
      () => (this.dataLoaded = true)
    );
  }

  private onSuccessDialog(message: string) {
    this.dataLoaded = true;
    this.dialog.open(SuccessfullActionDialogComponent, {
      data: { text: message }
    });
  }

  private onFailureDialog(message: string) {
    this.dataLoaded = true;
    this.dialog.open(FailedActionDialogComponent, {
      data: { text: message }
    });
  }

  private onConfirmDialog(message: string): Observable<boolean> {
    // let result: boolean;
    const dialogRef = this.dialog.open(ConfirmActionDialogComponent, {
      data: { text: message }
    });

    return dialogRef.afterClosed();
  }

  // getFullPlayers(
  //   competitionId: number,
  //   playerFilterOptions: PlayerFilterOptions,
  //   pageSize: number,
  //   pageNumber: number
  // ): void {
  //   this.dataLoaded = false;
  //   this.playerService
  //     .getPlayersWithScores(
  //       competitionId,
  //       playerFilterOptions,
  //       pageSize,
  //       pageNumber
  //     )
  //     .subscribe(
  //       (page: PageViewModel<PlayersWithScores>) => {
  //         this.log(page.toString());
  //         this.log(`Items: ${page.TotalCount}`);
  //         this.players = page.Items;
  //         this.playersCount = page.TotalCount;
  //       },
  //       error => this.onError(error), // Errors
  //       () => this.setDataSource() // Success
  //     );
  // }

  // getFullFilteredPlayers(): void {
  //   this.dataLoaded = false;
  //   this.getFullPlayers(
  //     this.competitionId,
  //     this.filter,
  //     this.defaultPageSize,
  //     this.defaultPageNumber
  //   );
  // }

  // onError(message: any) {
  //   this.dataLoaded = true;
  //   this.messageService.addError(message);
  // }

  // log(message: string): void {
  //   this.messageService.addLog(message);
  // }

  // setDataSource() {
  //   this.dataSource = new MatTableDataSource<PlayersWithScores>(this.players);
  //   this.dataLoaded = true;
  // }

  // onPageEvent(event: PageEvent) {
  //   this.defaultPageSize = event.pageSize;
  //   this.defaultPageNumber = event.pageIndex;
  //   this.getFullFilteredPlayers();
  // }

  // onSortEvent(event: Sort) {
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

  //   this.getFullFilteredPlayers();
  // }

  // prepareExtraColumns(): void {
  //   this.competition.ExtraColumns.map(column => column.Id).forEach(id =>
  //     this.displayedColumns.push(id.toString())
  //   );
  // }


  // public search(term: string): void {
  //   this.defaultPageNumber = 0;
  //   debounceTime(300);
  //   distinctUntilChanged();
  //   this.filter.Query = term;
  //   this.getFilteredPlayers();
  // }



  // private setPagetOptions() {
  //   const pageOptions = this.playerService.getPaginationInfo();
  //   this.defaultPageNumber = pageOptions.defaultPageNumber;
  //   this.defaultPageSize = pageOptions.defaultPageSize;
  //   this.pageSizeOptions = pageOptions.pageSizeOptions;
  // }
}
