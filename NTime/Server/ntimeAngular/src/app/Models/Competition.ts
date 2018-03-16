export class Competition {

    constructor(public Id:number, public City: string, public EventDate: Date, 
        public SignUpEndDate: Date) {

    }
    Name: string;
    Link: string;
    Organizer: string;
    OrganizerEditLock: boolean;
}
    //Id: number;
    //City: string;
    //EventDate: Date;