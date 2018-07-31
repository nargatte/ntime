import { Component, OnInit, ViewChild, AfterViewInit, Input } from '@angular/core';
import { FormControl, Validators, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { MatPaginator, MatTableDataSource, MatTable, PageEvent } from '@angular/material';
import { DataSource } from '@angular/cdk/table';
import { BehaviorSubject ,  Observable } from 'rxjs';



import { PlayerService } from '../../../Services/player.service';
import { MessageService } from '../../../Services/message.service';
import { PageViewModel } from '../../../Models/PageViewModel';
import { PlayerListView } from '../../../Models/Players/PlayerListView';
import { Competition } from '../../../Models/Competition';
import { PlayerFilterOptions } from '../../../Models/Players/PlayerFilterOptions';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-players-list',
  templateUrl: './players-list.component.html',
  styleUrls: ['./players-list.component.css']
})
export class PlayersListComponent implements AfterViewInit {
  @Input() competition: Competition;
  private competitionId: number;
  private players: PlayerListView[] = [];
  public todayDate: Date;
  private filter: PlayerFilterOptions = new PlayerFilterOptions();

  @ViewChild(MatTable) table: MatTable<PlayerListView>;
  displayedColumns = ['firstName', 'lastName', 'city', 'team', 'fullCategory', 'isPaidUp'];
  dataSource: MatTableDataSource<PlayerListView> = new MatTableDataSource<PlayerListView>(this.players);

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
  }

  ngAfterViewInit() {
    this.competitionId = +this.route.snapshot.paramMap.get('id');
    this.getPlayers(this.competitionId, this.filter, this.pageSize, this.pageNumber);
  }

  getPlayers(competitionId: number, playerFilterOptions: PlayerFilterOptions, pageSize: number, pageNumber: number): void {
    this.playerService.getPlayerListView(competitionId, playerFilterOptions, pageSize, pageNumber).subscribe(
      (page: PageViewModel<PlayerListView>) => {
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
    this.dataSource = new MatTableDataSource<PlayerListView>(this.players);
  }

  onPageEvent(event: PageEvent) {
    this.getPlayers(this.competition.Id, this.filter, event.pageSize, event.pageIndex);
  }
}
