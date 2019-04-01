import { ExtraColumn } from '../ExtraColumns/ExtraColumn';
import { ICompetition } from './ICompetition';

export class Competition implements ICompetition {
  public Name: string;
  public ExtraDataHeaders: string;
  public Link: string;
  public LinkDisplayedName: string;
  public Organizer: string;
  public OrganizerEditLock: boolean;
  public ExtraColumns: ExtraColumn[];

  constructor(
    public Id: number,
    public City: string,
    public EventDate: Date,
    public SignUpEndDate: Date
  ) {}

  public static convertDates(competition: Competition): Competition {
    competition.EventDate = new Date(competition.EventDate);
    competition.SignUpEndDate = new Date(competition.SignUpEndDate);
    return competition;
  }

  copyDataFromDto(dto: ICompetition): void {
    this.Id = dto.Id;
    this.Name = dto.Name;
    this.ExtraDataHeaders = dto.ExtraDataHeaders;
    this.Link = dto.Link;
    this.LinkDisplayedName = dto.LinkDisplayedName;
    this.Organizer = dto.Organizer;
    this.OrganizerEditLock = dto.OrganizerEditLock;
    this.ExtraColumns = dto.ExtraColumns;
  }
}
