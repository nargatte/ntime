import { Component, OnInit, Input } from '@angular/core';
import { Competition } from '../../Models/Competition';
import { CompetitionService } from '../../Services/competition.service';
import { MessageService } from '../../Services/message.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { PlayerCompetitionRegister } from '../../Models/PlayerCompetitionRegister';
import { ExtraPlayerInfo } from '../../Models/ExtraPlayerInfo';
import { Distance } from '../../Models/Distance';
import { PlayerService } from '../../Services/player.service';

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
  // public competition: Competition = new Competition(1, 'Łask', new Date(Date.now()),  new Date(2018, 9, 13));
  public newPlayer: PlayerCompetitionRegister = new PlayerCompetitionRegister();

  public checkboxes: boolean[] = [false, false, false];
  // competitionRegulationDeclarationText: string;
  // personalDataDeclarationText: string;
  // pressMediaDeclarationText: string;

  // competitionRegulationDeclarationCheckbox: boolean;
  // personalDataDeclarationCheckbox: boolean;
  // pressMediaDeclarationCheckbox: boolean;

  constructor(
    private competitionService: CompetitionService,
    private messageService: MessageService,
    private playerService: PlayerService,
  ) {
    this.todayDate = new Date(Date.now());
   }

  ngOnInit() {
    // this.heroForm = new FormGroup({
    //   'name': new FormControl(this.hero.name, [
    //     Validators.required,
    //     Validators.minLength(4),
    //   ]),
    //   'alterEgo': new FormControl(this.hero.alterEgo),
    //   'power': new FormControl(this.hero.power, Validators.required)
    // });

    // this.getCompetition();
  }


  // private getCompetition() {
  //   this.competitionService.getCompetition(this.id)
  //     .subscribe(c => this.competition = this.competition);
  // }

  public addPlayer() {
    console.log('Trying to add Player');
    this.playerService.addPlayer(this.newPlayer, this.competition.Id);
  }
}
