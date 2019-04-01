import { ExtraColumn } from '../ExtraColumns/ExtraColumn';
import { ICompetition } from './ICompetition';
import { BasicCompetitionModel } from 'src/app/MockData/mockCompetitions';

export class Competition implements ICompetition {
  public Id: number;
  public City: string;
  public EventDate: Date;
  public SignUpEndDate: Date;
  public Name: string;
  public ExtraDataHeaders: string;
  public Link: string;
  public LinkDisplayedName: string;
  public Organizer: string;
  public OrganizerEditLock: boolean;
  public ExtraColumns: ExtraColumn[];

  constructor(args?: BasicCompetitionModel) {
    if (args) {
      if (args.Id) {
        this.Id = args.Id;
      }
      if (args.City) {
        this.City = args.City;
      }
      if (args.EventDate) {
        this.EventDate = args.EventDate;
      }
      if (args.SignUpEndDate) {
        this.SignUpEndDate = args.SignUpEndDate;
      }
    }
  }

  copyDataFromDto(dto: ICompetition): Competition {
    this.Id = dto.Id;
    this.City = dto.City;
    this.EventDate = new Date(dto.EventDate);
    this.SignUpEndDate = new Date(dto.SignUpEndDate);
    this.Name = dto.Name;
    this.ExtraDataHeaders = dto.ExtraDataHeaders;
    this.Link = dto.Link;
    this.LinkDisplayedName = dto.LinkDisplayedName;
    this.Organizer = dto.Organizer;
    this.OrganizerEditLock = dto.OrganizerEditLock;
    this.ExtraColumns = dto.ExtraColumns.map(columnDto => new ExtraColumn().copyDataFromDto(columnDto));
    return this;
  }
}
