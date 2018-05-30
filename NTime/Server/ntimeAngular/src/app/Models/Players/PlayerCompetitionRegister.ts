export class PlayerCompetitionRegister {
    Id: number;
    FirstName: string;
    LastName: string;
    BirthDate: Date;
    IsMale: boolean;
    Team: string;
    PhoneNumber: string;
    City: string;
    Email: string;
    ExtraPlayerInfoId: number;
    DistanceId: number;
    CompetitionId: number;
    ReCaptchaToken: string;


    constructor(id: number, firstName: string, lastName: string) {
        this.Id = id;
        this.FirstName = firstName;
        this.LastName = lastName;
    }
}

