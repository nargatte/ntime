﻿import { AgeCategory } from '../AgeCategory';
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
        // if (isDevMode()) {
        //     console.log('Displaying player2:');
        //     console.log(this);
        //     console.log('Available age categories:');
        //     console.log(availableAgeCategories);
        // }
        const categoriesAfterSexFilter = availableAgeCategories
            .filter((ageCategory2) => new Date(this.BirthDate).getFullYear() >= ageCategory2.YearFrom
                && new Date(this.BirthDate).getFullYear() <= ageCategory2.YearTo);

        // if (isDevMode()) {
        //     console.log('Categories after sex filter::');
        //     console.log(categoriesAfterSexFilter);
        //     console.log(`Player's sex: ${this.IsMale}`);
        //     console.log(`Player's birthDate: ${new Date(this.BirthDate)}`);
        //     console.log(`Player's birthYear: ${new Date(this.BirthDate).getFullYear()}`);
        //     console.log(`Value of category sex ${String(categoriesAfterSexFilter[0].Male)}`);
        //     console.log(`Value of player sex ${String(this.IsMale)}`);
        // }
        const resolvedAgeCategories = categoriesAfterSexFilter
            .filter((ageCategory) => String(ageCategory.Male) === String(this.IsMale));

        // if (isDevMode()) {
        //     console.log('Resolved age categories:');
        //     console.log(resolvedAgeCategories);
        // }
        if (resolvedAgeCategories === null || resolvedAgeCategories.length === 0) {
            this.AgeCategoryId = 20;
            return null;
        } else {
            this.AgeCategoryId = resolvedAgeCategories[0].Id;
            return resolvedAgeCategories[0];
        }
    }
    // constructor(id?: number, firstName?: string, lastName?: string) {
    //     this.Id = id || 0;
    //     this.FirstName = firstName || '';
    //     this.LastName = lastName || '';
    // }

    // static fromSampleData(id?: number, firstName?: string, lastName?: string) {
    //     this.Id = id;
    //     this.FirstName = firstName || '';
    //     this.LastName = lastName || '';
    // }
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

