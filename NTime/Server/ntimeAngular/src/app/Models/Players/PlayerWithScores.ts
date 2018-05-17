export class PlayersWithScores {
    Id: number;
    FirstName: string;
    LastName: string;
    BirthDate: Date;
    IsMale: boolean;
    PhoneNumber: string;
    Team: string;
    StartNumber: number;
    StartTime: Date;
    City: string;
    IsPaidUp: boolean;
    Email: string;
    IsStartTimeFromReader: boolean;
    FullCategory: string;
    LapsCount: number;
    Time: number;
    DistancePlaceNumber: number;
    CategoryPlaceNumber: number;
    CompetitionCompleted: boolean;
    ExtraPlayerInfoId: number;
    DistanceId: number;
    AgeCategoryId: number;
    PlayerAccountId: number;

    constructor(id: number, firstName: string, lastName: string) {
        this.Id = id;
        this.FirstName = firstName;
        this.LastName = lastName;
    }
}
