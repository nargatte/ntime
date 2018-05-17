export class PlayerListView {
    Id: number;
    FirstName: string;
    LastName: string;
    IsMale: boolean;
    Team: string;
    StartNumber: number;
    StartTime: number;
    FullCategory: string;
    City: string;
    IsPaidUp: boolean;

    constructor(id: number, firstName: string, lastName: string) {
        this.Id = id;
        this.FirstName = firstName;
        this.LastName = lastName;
    }
 }
