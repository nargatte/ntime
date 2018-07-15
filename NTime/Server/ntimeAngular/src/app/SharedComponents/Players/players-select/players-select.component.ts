import { Component, OnInit, ViewChild, AfterViewInit, Input } from '@angular/core';
import { FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MatPaginator, MatTableDataSource, MatTable, PageEvent, MatDialog } from '@angular/material';
import { DataSource } from '@angular/cdk/table';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/merge';
import 'rxjs/add/operator/map';

import { PlayerService } from '../../../Services/player.service';
import { MessageService } from '../../../Services/message.service';
import { PageViewModel } from '../../../Models/PageViewModel';
import { PlayerListView } from '../../../Models/Players/PlayerListView';
import { Competition } from '../../../Models/Competition';
import { PlayerFilterOptions } from '../../../Models/Players/PlayerFilterOptions';
import { ActivatedRoute } from '@angular/router';
import { SelectionModel } from '@angular/cdk/collections';
import { SuccessfullActionDialogComponent } from '../../Dialogs/successfull-action-dialog/successfull-action-dialog.component';
import { FailedActionDialogComponent } from '../../Dialogs/failed-action-dialog/failed-action-dialog.component';
import { PlayersWithScores } from '../../../Models/Players/PlayerWithScores';

@Component({
  selector: 'app-players-select',
  templateUrl: './players-select.component.html',
  styleUrls: ['./players-select.component.css']
})
export class PlayersSelectComponent implements AfterViewInit {

  selection = new SelectionModel<PlayersWithScores>(true, []);

  @Input() competition: Competition;
  private competitionId: number;
  private players: PlayersWithScores[] = [];
  public todayDate: Date;
  private filter: PlayerFilterOptions = new PlayerFilterOptions();

  @ViewChild(MatTable) table: MatTable<PlayerListView>;
  displayedColumns = ['select', 'firstName', 'lastName', 'sex', 'team', 'fullCategory', 'isPaidUp'];
  dataSource: MatTableDataSource<PlayersWithScores> = new MatTableDataSource<PlayersWithScores>(this.players);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  public pageNumber = 0;
  public pageSize = 20;
  public pageSizeOptions = [10, 20, 50];
  public playersCount = 0;

  constructor(
    private playerService: PlayerService,
    private messageService: MessageService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
  ) {
    this.todayDate = new Date(Date.now());
  }

  ngAfterViewInit() {
    this.competitionId = +this.route.snapshot.paramMap.get('id');
    this.getFullPlayers(this.competitionId, this.filter, this.pageSize, this.pageNumber);
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  masterToggle() {
    this.isAllSelected() ?
      this.selection.clear() :
      this.dataSource.data.forEach(row => this.selection.select(row));
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

  getFullPlayers(competitionId: number, playerFilterOptions: PlayerFilterOptions, pageSize: number, pageNumber: number): void {
    this.playerService.getPlayersWithScores(competitionId, playerFilterOptions, pageSize, pageNumber).subscribe(
      (page: PageViewModel<PlayersWithScores>) => {
        this.log(page.toString());
        this.log(`Items: ${page.TotalCount}`);
        this.players = page.Items;
        this.playersCount = page.TotalCount;
      },
      error => this.log(error), // Errors
      () => this.setDataSource() // Success
    );
  }

  log(message: string): void {
    this.messageService.addLog(message);
  }

  setDataSource() {
    this.dataSource = new MatTableDataSource<PlayersWithScores>(this.players);
  }

  onPageEvent(event: PageEvent) {
    this.pageSize = event.pageSize;
    this.pageNumber = event.pageIndex;
    this.getFullPlayers(this.competition.Id, this.filter, this.pageSize, this.pageNumber);
  }


  public paidButtonClick(): void {
    this.saveSelectedPlayersPaid(true);
  }

  public notPaidButtonClick(): void {
    this.saveSelectedPlayersPaid(false);
  }

  private saveSelectedPlayersPaid(isPaid: boolean) {
    const selectedPlayers = this.selection.selected;
    selectedPlayers.forEach(player => {
      player.IsPaidUp = isPaid;
    });
    this.playerService.updateMulitplePlayers(selectedPlayers).subscribe(
      result => {
        this.selection.clear();
        this.getFullPlayers(this.competition.Id, this.filter, this.pageSize, this.pageNumber);
        this.dialog.open(SuccessfullActionDialogComponent, {
          data: { text: 'Zmiany zostały zapisane' }
        });
      },
      error => {
        this.messageService.addError(`Could not save changes`);
        this.messageService.addObject(error);
        this.dialog.open(FailedActionDialogComponent, {
          data: { text: 'Nie udało się zapisać zmian' }
        });
      }
    );
  }
}