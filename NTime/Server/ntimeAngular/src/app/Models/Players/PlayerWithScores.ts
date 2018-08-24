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

    public copyDataFromFullDto(playerDto: PlayerWithScoresDto) {
        super.copyDataFromDto(playerDto);
        this.StartNumber = playerDto.StartNumber;
        this.StartTime = playerDto.StartTime;
        this.IsPaidUp = playerDto.IsPaidUp;
        this.IsStartTimeFromReader = playerDto.IsStartTimeFromReader;
        this.FullCategory = playerDto.FullCategory;
        this.LapsCount = playerDto.LapsCount;
        this.Time = playerDto.Time;
        this.DistancePlaceNumber = playerDto.DistancePlaceNumber;
        this.CategoryPlaceNumber = playerDto.CategoryPlaceNumber;
        this.CompetitionCompleted = playerDto.CompetitionCompleted;
        this.PlayerAccountId = playerDto.PlayerAccountId;
    }
}
