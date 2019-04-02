import { IPlayerPublicDetails } from './IPlayerPublicDetails';

export interface IPlayerWithScores extends IPlayerPublicDetails {
  BirthDate: Date;
  PhoneNumber: string;
  Email: string;
  IsStartTimeFromReader: boolean;
  IsCategoryFixed: boolean;
}
