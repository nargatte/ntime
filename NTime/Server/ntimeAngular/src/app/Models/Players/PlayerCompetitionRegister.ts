export class PlayerCompetitionRegister {
    Id: number;
    FirstName: string;
    LastName: string;
    BirthDate: Date;
    IsMale: boolean;
    Team: string;
    PhoneNumber: string;
    ExtraData: string;
    City: string;
    Email: string;
    SubcategoryId: number;
    DistanceId: number;
    CompetitionId: number;
    ReCaptchaToken: string;

    constructor(args?: BasicPlayerArguments) {
        if (args) {
            if (args.Id) {
                this.Id = args.Id;
            }
            if (args.FirstName) {
                this.FirstName = args.FirstName;
            }
            if (args.LastName) {
                this.LastName = args.LastName;
            }
        }
    }
    // constructor(id?: number, firstName?: string, lastName?: string) {
    //     this.Id = id || 0;
    //     this.FirstName = firstName || '';
    //     this.LastName = lastName || '';
    // }

    // static fromSampleData(id?: number, firstName?: string, lastName?: string) {
    //     this.Id = id;
    //     this.FirstName = firstName || '';
    //     this.LastName = lastName || '';
    // }
}

export class BasicPlayerArguments {

    constructor(id: number, firstName: string, lastName: string) {
        this.Id = id;
        this.FirstName = firstName;
        this.LastName = lastName;
    }
    Id: number;
    FirstName: string;
    LastName: string;
}

