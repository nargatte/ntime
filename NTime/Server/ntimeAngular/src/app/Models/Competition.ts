export class Competition {

    constructor(public Id: number, public City: string, public EventDate: Date,
        public SignUpEndDate: Date) {

    }
    Name: string;
    Link: string;
    Organizer: string;
    OrganizerEditLock: boolean;

    public static convertDates(competition: Competition): Competition {
        competition.EventDate = new Date(competition.EventDate);
        competition.SignUpEndDate = new Date(competition.SignUpEndDate);
        return competition;
    }
}
