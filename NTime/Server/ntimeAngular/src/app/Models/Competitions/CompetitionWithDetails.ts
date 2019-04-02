import { AgeCategory } from '../AgeCategory';
import { Distance } from '../Distance';
import { Subcategory } from '../Subcategory';
import { AgeCategoryDistance } from '../AgeCategoryDistance';
import { Competition } from './Competition';
import { ICompetitionWithDetails } from './ICompetitionWithDetails';
import { BasicCompetitionModel } from '../../MockData/mockCompetitions';

export class CompetitionWithDetails extends Competition
  implements ICompetitionWithDetails {
  AgeCategories: AgeCategory[];
  Distances: Distance[];
  Subcategories: Subcategory[];
  AgeCategoryDistances: AgeCategoryDistance[];

  constructor(args?: BasicCompetitionModel
  ) {
    super(args);
  }

  public static convertDates(
    competition: CompetitionWithDetails
  ): CompetitionWithDetails {
    competition.EventDate = new Date(competition.EventDate);
    competition.SignUpEndDate = new Date(competition.SignUpEndDate);
    return competition;
  }

  copyDataFromDto(dto: ICompetitionWithDetails): CompetitionWithDetails {
    super.copyDataFromDto(dto);
    this.AgeCategories = dto.AgeCategories;
    this.Distances = dto.Distances;
    this.Subcategories = dto.Subcategories;
    this.AgeCategoryDistances = dto.AgeCategoryDistances;
    return this;
  }
}
