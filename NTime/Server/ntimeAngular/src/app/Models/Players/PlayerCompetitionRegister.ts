import { AgeCategory } from '../AgeCategory';
import { PlayerListView } from './PlayerListView';

export class PlayerCompetitionRegister extends PlayerListView
  implements IPlayerCompetitionRegister {
  public Email: string;
  public BirthDate: Date;
  public PhoneNumber: string;
  public SubcategoryId: number;
  public DistanceId: number;
  public AgeCategoryId: number;
  public ReCaptchaToken: string;

  constructor(args?: BasicPlayerArguments) {
    super(args.Id, args.FirstName, args.LastName);
    // if (args) {
    //   if (args.Id) {
    //     this.Id = args.Id;
    //   }
    //   if (args.FirstName) {
    //     this.FirstName = args.FirstName;
    //   }
    //   if (args.LastName) {
    //     this.LastName = args.LastName;
    //   }
    // }
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

  copyDataFromDto(dto: IPlayerCompetitionRegister) {
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
  }
}

export class BasicPlayerArguments {
  constructor(id: number, firstName: string, lastName: string) {
    this.Id = id;
    this.FirstName = firstName;
    this.LastName = lastName;
  }
  Id: number;
  FirstName: string;
  LastName: string;
}
