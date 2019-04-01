import { AgeCategory } from '../AgeCategory';
import { Distance } from '../Distance';
import { Subcategory } from '../Subcategory';
import { AgeCategoryDistance } from '../AgeCategoryDistance';
import { ICompetition } from './ICompetition';

export interface ICompetitionWithDetails extends ICompetition {
  AgeCategories: AgeCategory[];
  Distances: Distance[];
  Subcategories: Subcategory[];
  AgeCategoryDistances: AgeCategoryDistance[];
}
