import { PlayerCompetitionRegister, BasicPlayerArguments } from './PlayerCompetitionRegister';

export class PlayersWithScores extends PlayerCompetitionRegister {
    StartNumber: number;
    StartTime: Date;
    IsPaidUp: boolean;
    IsStartTimeFromReader: boolean;
    FullCategory: string;
    LapsCount: number;
    Time: number;
    DistancePlaceNumber: number;
    CategoryPlaceNumber: number;
    CompetitionCompleted: boolean;
    PlayerAccountId: number;

    constructor(args?: BasicPlayerArguments) {
        super(args);
    }
}
