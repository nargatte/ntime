import { AgeCategory } from '../AgeCategory';
import { MessageService } from '../../Services/message.service';
import { isDevMode } from '../../../../node_modules/@angular/core';

export class PlayerCompetitionRegister {
    public Id: number;
    public FirstName: string;
    public LastName: string;
    public BirthDate: Date;
    public IsMale: boolean;
    public Team: string;
    public PhoneNumber: string;
    public ExtraData: string;
    public City: string;
    public Email: string;
    public SubcategoryId: number;
    public DistanceId: number;
    public AgeCategoryId: number;
    public CompetitionId: number;
    public ReCaptchaToken: string;

    constructor(args?: BasicPlayerArguments) {
        if (args) {
            if (args.Id) {
                this.Id = args.Id;
            }
            if (args.FirstName) {
                this.FirstName = args.FirstName;
            }
            if (args.LastName) {
                this.LastName = args.LastName;
            }
        }
    }

    public resolveAgeCategory(availableAgeCategories: AgeCategory[]): AgeCategory {
        const categoriesAfterSexFilter = availableAgeCategories
            .filter((ageCategory2) => new Date(this.BirthDate).getFullYear() >= ageCategory2.YearFrom
                && new Date(this.BirthDate).getFullYear() <= ageCategory2.YearTo);

        const resolvedAgeCategories = categoriesAfterSexFilter
            .filter((ageCategory) => String(ageCategory.Male) === String(this.IsMale));

        if (resolvedAgeCategories === null || resolvedAgeCategories.length === 0) {
            return null;
        } else {
            this.AgeCategoryId = resolvedAgeCategories[0].Id;
            return resolvedAgeCategories[0];
        }
    }

    copyDataFromDto(playerDto: PlayerCompetitionRegisterDto) {
        this.Id = playerDto.Id;
        this.FirstName = playerDto.FirstName;
        this.LastName = playerDto.LastName;
        this.BirthDate = playerDto.BirthDate;
        this.IsMale = playerDto.IsMale;
        this.Team = playerDto.Team;
        this.PhoneNumber = playerDto.PhoneNumber;
        this.ExtraData = playerDto.ExtraData;
        this.City = playerDto.City;
        this.Email = playerDto.Email;
        this.SubcategoryId = playerDto.SubcategoryId;
        this.DistanceId = playerDto.DistanceId;
        this.AgeCategoryId = playerDto.AgeCategoryId;
        this.CompetitionId = playerDto.CompetitionId;
        this.ReCaptchaToken = playerDto.ReCaptchaToken;
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

