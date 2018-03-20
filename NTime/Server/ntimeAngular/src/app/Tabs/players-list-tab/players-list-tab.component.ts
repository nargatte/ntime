import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PlayerService } from '../../Services/player.service';
import { PlayerListView } from '../../Models/PlayerListView';
import { PlayerFilterOptions } from '../../Models/PlayerFilterOptions';
import { PageViewModel } from '../../Models/PageViewModel';
import { COMPETITIONS } from '../../MockData/mockCompetitions';
import { Competition } from '../../Models/Competition';
import { CompetitionService } from '../../Services/competition.service';

@Component({
  selector: 'app-players-list-tab',
  templateUrl: './players-list-tab.component.html',
  styleUrls: ['./players-list-tab.component.css', '../tab-style.css']
})
export class PlayersListTabComponent implements OnInit, AfterViewInit {

  public competitionId: number;
  public competition: Competition = COMPETITIONS[0];
  public players: PlayerListView[] = [];
  public todayDate: Date;

  constructor(
    private route: ActivatedRoute,
    private competitionService: CompetitionService,
    private playerListService: PlayerService) {
    this.todayDate = new Date(Date.now());
  }

  ngOnInit() {
    // this.getPlayers();
    this.competitionId = +this.route.snapshot.paramMap.get('id');
    this.getCompetition(this.competitionId);
  }

  ngAfterViewInit(): void {

  }

  getCompetition(id: number): void {
    this.competitionService.getCompetition(id).subscribe(
      (competition: Competition) => {
        // console.log(`Competition received ${competition}`);
        this.competition = Competition.convertDates(competition); // TODO: Try to make not static
      },
      error => console.log(error), // Errors
      // () => console.log('Succes getting data') // Success
    );
  }



  // private getPlayers() {
  //   this.playerListService.getPlayerListView(this.competitionId, this.filter, 10, 0).subscribe((page: PageViewModel<PlayerListView>) => {
  //     this.players = page.Items;
  //   });
  // }
}
