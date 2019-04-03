import { PlayerPublicDetails } from './PlayerPublicDetails';
import { BasicPlayerArguments } from './BasicPlayerArguments';
import { AgeCategory } from '../AgeCategory';
import { IPlayerWithScores } from './IPlayerWithScores';
import { ExtraColumnValue } from '../ExtraColumns/ExtraColumnValue';

export class PlayersWithScores extends PlayerPublicDetails
  implements IPlayerWithScores {
  BirthDate: Date;
  PhoneNumber: string;
  Email: string;
  IsStartTimeFromReader: boolean;
  IsCategoryFixed: boolean;

  constructor(args?: BasicPlayerArguments,
    extraColumnValues?: ExtraColumnValue[]) {
    super(args, extraColumnValues);
  }

  public copyDataFromFullDto(playerDto: IPlayerWithScores): PlayersWithScores {
    super.copyDataFromDto(playerDto);
    this.BirthDate = new Date(playerDto.BirthDate);
    this.PhoneNumber = playerDto.PhoneNumber;
    this.Email = playerDto.Email;
    this.IsStartTimeFromReader = playerDto.IsStartTimeFromReader;
    this.IsCategoryFixed = playerDto.IsCategoryFixed;
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
