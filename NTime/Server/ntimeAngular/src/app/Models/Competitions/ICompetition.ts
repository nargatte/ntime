import { ExtraColumn } from '../ExtraColumns/ExtraColumn';

export interface ICompetition extends IDtoBase<ICompetition> {
  Id: number;
  City: string;
  EventDate: Date;
  SignUpEndDate: Date;
  Name: string;
  ExtraDataHeaders: string;
  Link: string;
  LinkDisplayedName: string;
  Organizer: string;
  OrganizerEditLock: boolean;
  ExtraColumns: ExtraColumn[];
}
