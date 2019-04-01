import { Competition } from '../Models/Competitions/Competition';
import { PageViewModel } from '../Models/PageViewModel';

export class BasicCompetitionModel {
  constructor(
    public Id: number,
    public City: string,
    public EventDate: Date,
    public SignUpEndDate: Date
  ) {}
}

export const MockCompetitions: Competition[] = [
  new Competition(new BasicCompetitionModel(
    9999,
    'MockCompetition',
    new Date(Date.now()),
    new Date(2018, 9, 13)
  )),
  new Competition(new BasicCompetitionModel(2, 'Warszawa', new Date(2018, 8, 25), new Date(2018, 9, 13))),
  new Competition(new BasicCompetitionModel(3, 'Szczecin', new Date(2018, 9, 13), new Date(2018, 9, 13)))
];
export const COMPETITIONS_PAGE: PageViewModel<Competition>[] = [
  new PageViewModel<Competition>(3, MockCompetitions)
];


