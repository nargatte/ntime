interface PlayerWithScoresDto extends PlayerCompetitionRegisterDto {
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
}
