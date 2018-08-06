import { Competition } from '../Models/Competition';
import { PageViewModel } from '../Models/PageViewModel';

export const MockCompetitions: Competition[] = [
    new Competition(9999, 'MockCompetition', new Date(Date.now()),  new Date(2018, 9, 13)),
    new Competition(2, 'Warszawa', new Date(2018, 8, 25),  new Date(2018, 9, 13)),
    new Competition(3, 'Szczecin', new Date(2018, 9, 13),  new Date(2018, 9, 13)),
];
export const COMPETITIONS_PAGE: PageViewModel<Competition>[] = [
        new PageViewModel<Competition>(3, MockCompetitions),
];
