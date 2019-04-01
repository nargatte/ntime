import { PlayerListView } from '../Models/Players/PlayerListView';
import { PageViewModel } from '../Models/PageViewModel';
import { PlayersWithScores } from '../Models/Players/PlayerWithScores';
import { PlayerCompetitionRegister } from '../Models/Players/PlayerCompetitionRegister';
import { BasicPlayerArguments } from '../Models/Players/BasicPlayerArguments';


export const MockPlayersListView: PlayerListView[] = [
    new PlayerListView(new BasicPlayerArguments( 1, 'Jan', 'Kowalski')),
    new PlayerListView(new BasicPlayerArguments( 2, 'Marek', 'Tokarczyk')),
    new PlayerListView(new BasicPlayerArguments( 3, 'Jadwiga', 'Storczyk'))
];
export const MockPlayersListViewPage: PageViewModel<PlayerListView> =
    new PageViewModel<PlayerListView>(3, MockPlayersListView);

// export const MockPlayersWithScores: PlayersWithScores[] = [
//     new PlayersWithScores(new BasicPlayerArguments( 1, 'Jan', 'Kowalski')),
//     new PlayersWithScores(new BasicPlayerArguments(2, 'Marek', 'Tokarczyk')),
//     new PlayersWithScores(new BasicPlayerArguments(3, 'Jadwiga', 'Storczyk'))
// ];
// export const MockPlayersWithScoresPage: PageViewModel<PlayersWithScores> =
//     new PageViewModel<PlayersWithScores>(3, MockPlayersWithScores);

// export const MockPlayersCompetitionRegister: PlayerCompetitionRegister[] = [
//     new PlayerCompetitionRegister(new BasicPlayerArguments(1, 'Jan', 'Kowalski')),
//     new PlayerCompetitionRegister(new BasicPlayerArguments(2, 'Marek', 'Tokarczyk')),
//     new PlayerCompetitionRegister(new BasicPlayerArguments(3, 'Jadwiga', 'Storczyk')),
// ];
// export const MockPlayersCompetitionRegisterPage: PageViewModel<PlayerCompetitionRegister> =
//     new PageViewModel<PlayerCompetitionRegister>(3, MockPlayersCompetitionRegister);

