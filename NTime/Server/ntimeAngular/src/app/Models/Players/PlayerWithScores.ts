import { PlayerPublicDetails } from './PlayerPublicDetails';
import { BasicPlayerArguments } from './BasicPlayerArguments';
import { AgeCategory } from '../AgeCategory';
import { IPlayerWithScores } from './IPlayerWithScores';

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

  public copyDataFromFullDto(playerDto: IPlayerWithScores): PlayersWithScores {
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
    return this;
  }

  public resolveAgeCategory(
    availableAgeCategories: AgeCategory[]
  ): AgeCategory {
    const categoriesAfterSexFilter = availableAgeCategories.filter(
      ageCategory2 =>
        new Date(this.BirthDate).getFullYear() >= ageCategory2.YearFrom &&
        new Date(this.BirthDate).getFullYear() <= ageCategory2.YearTo
    );

    const resolvedAgeCategories = categoriesAfterSexFilter.filter(
      ageCategory => String(ageCategory.Male) === String(this.IsMale)
    );

    if (resolvedAgeCategories === null || resolvedAgeCategories.length === 0) {
      return null;
    } else {
      this.AgeCategoryId = resolvedAgeCategories[0].Id;
      return resolvedAgeCategories[0];
    }
  }
}
