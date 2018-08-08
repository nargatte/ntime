import { Competition } from './Competitions/Competition';


export class OrganizerAccount {
    public Id: number;
    public FirstName: string;
    public LastName: string;
    public PhoneNumber: string;
    public EMail: string;
    public CompetitionDtos: Competition[];
}
