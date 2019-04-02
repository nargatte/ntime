import { AgeCategory } from '../AgeCategory';
import { PlayerListView } from './PlayerListView';
import { BasicPlayerArguments } from './BasicPlayerArguments';
import { IPlayerCompetitionRegister } from './IPlayerCompetitionRegister';
import { ExtraColumnValue } from '../ExtraColumns/ExtraColumnValue';

export class PlayerCompetitionRegister extends PlayerListView
  implements IPlayerCompetitionRegister {
  public Email: string;
  public BirthDate: Date;
  public PhoneNumber: string;
  public SubcategoryId: number;
  public DistanceId: number;
  public AgeCategoryId: number;
  public ReCaptchaToken: string;

  constructor(args?: BasicPlayerArguments, extraColumnValues?: ExtraColumnValue[]) {
    super(args, extraColumnValues);
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

  copyDataFromDto(dto: IPlayerCompetitionRegister): PlayerCompetitionRegister {
    super.copyDataFromDto(dto);
    this.PhoneNumber = dto.PhoneNumber;
    this.BirthDate = dto.BirthDate;
    this.ExtraData = dto.ExtraData;
    this.Email = dto.Email;
    this.SubcategoryId = dto.SubcategoryId;
    this.DistanceId = dto.DistanceId;
    this.AgeCategoryId = dto.AgeCategoryId;
    this.CompetitionId = dto.CompetitionId;
    this.ReCaptchaToken = dto.ReCaptchaToken;
    return this;
  }
}
