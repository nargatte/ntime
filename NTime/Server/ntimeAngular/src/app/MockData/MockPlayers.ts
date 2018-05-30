import { PlayerListView } from '../Models/Players/PlayerListView';
import { PageViewModel } from '../Models/PageViewModel';
import { PlayersWithScores } from '../Models/Players/PlayerWithScores';
import { PlayerCompetitionRegister } from '../Models/Players/PlayerCompetitionRegister';


export const MockPlayersListView: PlayerListView[] = [
    new PlayerListView(1, 'Jan', 'Kowalski'),
    new PlayerListView(2, 'Marek', 'Tokarczyk'),
    new PlayerListView(3, 'Jadwiga', 'Storczyk')
];
export const MockPlayersListViewPage: PageViewModel<PlayerListView> =
    new PageViewModel<PlayerListView>(3, MockPlayersListView);

export const MockPlayersWithScores: PlayersWithScores[] = [
    new PlayersWithScores(1, 'Jan', 'Kowalski'),
    new PlayersWithScores(2, 'Marek', 'Tokarczyk'),
    new PlayersWithScores(3, 'Jadwiga', 'Storczyk')
];
export const MockPlayersWithScoresPage: PageViewModel<PlayersWithScores> =
    new PageViewModel<PlayersWithScores>(3, MockPlayersWithScores);

export const MockPlayersCompetitionRegister: PlayerCompetitionRegister[] = [
    new PlayerCompetitionRegister(1, 'Jan', 'Kowalski'),
    new PlayerCompetitionRegister(2, 'Marek', 'Tokarczyk'),
    new PlayerCompetitionRegister(3, 'Jadwiga', 'Storczyk')
];
export const MockPlayersCompetitionRegisterPage: PageViewModel<PlayerCompetitionRegister> =
    new PageViewModel<PlayerCompetitionRegister>(3, MockPlayersCompetitionRegister);

