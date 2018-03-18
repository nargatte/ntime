import { Component, OnInit } from '@angular/core';
import { Competition } from '../../Models/Competition';
import { CompetitionService } from '../../Services/competition.service';
import { MessageService } from '../../Services/message.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { PlayerCompetitionRegister } from '../../Models/PlayerCompetitionRegister';

@Component({
  selector: 'app-new-player-form',
  templateUrl: './new-player-form.component.html',
  styleUrls: ['./new-player-form.component.css']
})
export class NewPlayerFormComponent implements OnInit {
  public id = 2;
  public competition: Competition = new Competition(1, 'Łask', new Date(Date.now()),  new Date(2018, 9, 13));
  public newPlayer: PlayerCompetitionRegister = new PlayerCompetitionRegister();
  public checkboxes: boolean[] = [false, false, false];
  // competitionRegulationDeclarationText: string;
  // personalDataDeclarationText: string;
  // pressMediaDeclarationText: string;

  competitionRegulationDeclarationCheckbox: boolean;
  personalDataDeclarationCheckbox: boolean;
  pressMediaDeclarationCheckbox: boolean;
  powers = ['Really Smart', 'Super Flexible', 'Weather Changer'];

  hero = {name: 'Dr.', alterEgo: 'Dr. What', power: this.powers[0]};

  heroForm: FormGroup;

  constructor(private competitionService: CompetitionService, private messageService: MessageService) { }

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


  private getCompetition() {
    this.competitionService.getCompetition(this.id)
      .subscribe(competition => this.competition = this.competition);
  }

  public addPlayer() {
    this.messageService.addLog('Zawodnik został zapisany na zawody');
  }
}
