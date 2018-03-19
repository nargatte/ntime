import { Component, OnInit, Input } from '@angular/core';
import { Competition } from '../../Models/Competition';
import { CompetitionService } from '../../Services/competition.service';
import { MessageService } from '../../Services/message.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { PlayerCompetitionRegister } from '../../Models/PlayerCompetitionRegister';
import { ExtraPlayerInfo } from '../../Models/ExtraPlayerInfo';
import { Distance } from '../../Models/Distance';
import { PlayerService } from '../../Services/player.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-new-player-form',
  templateUrl: './new-player-form.component.html',
  styleUrls: ['./new-player-form.component.css']
})
export class NewPlayerFormComponent implements OnInit {
  @Input() competition: Competition;
  @Input() distances: Distance[];
  @Input() extraPlayerInfos: ExtraPlayerInfo[];

  public todayDate: Date;
  public newPlayer: PlayerCompetitionRegister = new PlayerCompetitionRegister();
  private competitionId: number;

  public checkboxes: boolean[] = [false, false, false];

  constructor(
    private route: ActivatedRoute,
    private competitionService: CompetitionService,
    private messageService: MessageService,
    private playerService: PlayerService,
  ) {
    this.competitionId = +this.route.snapshot.paramMap.get('id');
    this.todayDate = new Date(Date.now());
   }

  ngOnInit() {

  }

  log(message: string): void {
    this.messageService.addLog(message);
  }

  // private getCompetition() {
  //   this.competitionService.getCompetition(this.id)
  //     .subscribe(c => this.competition = this.competition);
  // }

  public addPlayer() {
    this.newPlayer.DistanceId = 27;
    console.log('Trying to add Player');
    this.playerService.addPlayer(this.newPlayer, this.competitionId).subscribe(
      player => this.log(`Dodano zawodnika ${player}`),
      error => this.log(`Wystąpił problem podczas dodawania zawodnika: ${error}`)
    );
  }
}
