import { BasicPlayerArguments } from './PlayerCompetitionRegister';
import { PlayerPublicDetails } from './PlayerPublicDetails';

export class PlayersWithScores extends PlayerPublicDetails
  implements IPlayerWithScores {
  BirthDate: Date;
  PhoneNumber: string;
  Email: string;
  IsStartTimeFromReader: boolean;
  IsCategoryFixed: boolean;

  constructor(args?: BasicPlayerArguments) {
    super(args);
  }

  public copyDataFromFullDto(playerDto: IPlayerWithScores) {
    super.copyDataFromDto(playerDto);
    this.StartNumber = playerDto.StartNumber;
    this.StartTime = playerDto.StartTime;
    this.IsPaidUp = playerDto.IsPaidUp;
    this.IsStartTimeFromReader = playerDto.IsStartTimeFromReader;
    this.FullCategory = playerDto.FullCategory;
    this.LapsCount = playerDto.LapsCount;
    this.Time = playerDto.Time;
    this.DistancePlaceNumber = playerDto.DistancePlaceNumber;
    this.CategoryPlaceNumber = playerDto.CategoryPlaceNumber;
    this.CompetitionCompleted = playerDto.CompetitionCompleted;
    this.PlayerAccountId = playerDto.PlayerAccountId;
  }
}
