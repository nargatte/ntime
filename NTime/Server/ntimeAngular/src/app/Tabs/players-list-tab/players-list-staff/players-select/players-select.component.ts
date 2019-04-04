import {
  Component,
  OnInit,
  ViewChild,
  AfterViewInit,
  Input
} from '@angular/core';
import { FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {
  MatPaginator,
  MatTableDataSource,
  MatTable,
  PageEvent,
  MatDialog,
  Sort
} from '@angular/material';
import { DataSource } from '@angular/cdk/table';
import { BehaviorSubject, Observable } from 'rxjs';
import { String, StringBuilder } from 'typescript-string-operations';

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
import { ExtraDataDefinition } from '../../../../Models/CDK/ExtraDataDefinition';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { ConfirmActionDialogComponent } from '../../../../SharedComponents/Dialogs/confirm-action-dialog/confirm-action-dialog.component';

@Component({
  selector: 'app-players-select',
  templateUrl: './players-select.component.html',
  styleUrls: [
    './players-select.component.css',
    '../../../../app.component.css',
    '../../../../Styles/mobile-style.css'
  ]
})
export class PlayersSelectComponent implements OnInit, AfterViewInit {
  selection = new SelectionModel<PlayersWithScores>(true, []);

  @Input() competition: Competition;
  public competitionId: number;
  private players: PlayersWithScores[] = [];
  public todayDate: Date;
  private filter: PlayerFilterOptions = new PlayerFilterOptions();
  private delimiter = '|';
  public dataLoaded = false;

  @ViewChild(MatTable) table: MatTable<PlayerListView>;
  displayedColumns = [
    'select',
    'firstName',
    'lastName',
    'city',
    'team',
    'fullCategory',
    'isPaidUp'
  ];
  oldExtraColumns: ExtraDataDefinition[] = [];
  dataSource: MatTableDataSource<PlayersWithScores> = new MatTableDataSource<
    PlayersWithScores
  >(this.players);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  public pageNumber = 0;
  public pageSize = 50;
  public pageSizeOptions = [20, 50, 100];
  public playersCount = 0;

  constructor(
    private playerService: PlayerService,
    private messageService: MessageService,
    private route: ActivatedRoute,
    private dialog: MatDialog
  ) {
    this.todayDate = new Date(Date.now());
    this.competitionId = +this.route.snapshot.paramMap.get('id');
  }

  ngOnInit() {
    this.setDefaultSorting();
    this.getFullFilteredPlayers();
    this.prepareExtraColumns();
    this.addActionsColumns();
  }

  ngAfterViewInit() {}

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

  // getPlayersList(competitionId: number, playerFilterOptions: PlayerFilterOptions, pageSize: number, pageNumber: number): void {
  //   this.playerService.getPlayerListView(competitionId, playerFilterOptions, pageSize, pageNumber).subscribe(
  //     (page: PageViewModel<PlayerListView>) => {
  //       this.log(page.toString());
  //       this.log(`Items: ${page.TotalCount}`);
  //       this.players = page.Items;
  //       this.playersCount = page.TotalCount;
  //     },
  //     error => this.log(error), // Errors
  //     () => this.setDataSource() // Success
  //   );
  // }

  getFullPlayers(
    competitionId: number,
    playerFilterOptions: PlayerFilterOptions,
    pageSize: number,
    pageNumber: number
  ): void {
    this.dataLoaded = false;
    this.playerService
      .getPlayersWithScores(
        competitionId,
        playerFilterOptions,
        pageSize,
        pageNumber
      )
      .subscribe(
        (page: PageViewModel<PlayersWithScores>) => {
          this.log(page.toString());
          this.log(`Items: ${page.TotalCount}`);
          this.players = page.Items;
          this.playersCount = page.TotalCount;
        },
        error => this.onError(error), // Errors
        () => this.setDataSource() // Success
      );
  }

  getFullFilteredPlayers(): void {
    this.dataLoaded = false;
    this.getFullPlayers(
      this.competitionId,
      this.filter,
      this.pageSize,
      this.pageNumber
    );
  }

  onError(message: any) {
    this.dataLoaded = true;
    this.messageService.addError(message);
  }

  log(message: string): void {
    this.messageService.addLog(message);
  }

  setDataSource() {
    this.dataLoaded = true;
    this.dataSource = new MatTableDataSource<PlayersWithScores>(this.players);
  }

  onPageEvent(event: PageEvent) {
    this.pageSize = event.pageSize;
    this.pageNumber = event.pageIndex;
    this.getFullFilteredPlayers();
  }

  onSortEvent(event: Sort) {
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

    this.getFullFilteredPlayers();
  }

  setDefaultSorting() {
    this.filter.PlayerSort = PlayerSort.ByLastName;
    this.filter.DescendingSort = false;
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
      result => {
        this.selection.clear();
        this.getFullPlayers(
          this.competition.Id,
          this.filter,
          this.pageSize,
          this.pageNumber
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

  prepareExtraColumns(): void {
    // this.messageService.addObject(this.competition);
    // this.messageService.addLog(
    //   `ExtraDataHeaders: ${this.competition.ExtraDataHeaders}`
    // );
    // if (String.IsNullOrWhiteSpace(this.competition.ExtraDataHeaders)) {
    //   return;
    // }

    // const splitColumns = this.competition.ExtraDataHeaders.split(
    //   this.delimiter
    // );
    // let iterator = 0;
    // splitColumns.forEach(columnString => {
    //   this.oldExtraColumns.push(
    //     new ExtraDataDefinition(
    //       iterator.toString(),
    //       columnString,
    //       iterator,
    //       this.delimiter
    //     )
    //   );
    //   iterator++;
    // });

    // this.oldExtraColumns
    //   .map(x => x.columnDef)
    //   .forEach(c => this.displayedColumns.push(c));

    this.competition.ExtraColumns.map(column => column.Id).forEach(id =>
      this.displayedColumns.push(id.toString())
    );
  }

  private addActionsColumns(): void {
    this.displayedColumns.push('editPlayer');
    this.displayedColumns.push('deletePlayer');
  }

  public search(term: string): void {
    this.pageNumber = 0;
    debounceTime(300);
    distinctUntilChanged();
    this.filter.Query = term;
    this.getFullFilteredPlayers();
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
      (player: PlayersWithScores) => {
        this.onSuccessDialog('Zawodnik został usunięty');
        this.getFullFilteredPlayers();
      },
      (error: Error) =>
        this.onFailureDialog(
          `Nie udało się usunąć zawodnika: ${error.message}`
        ),
      () => (this.dataLoaded = true)
    );
  }
}
