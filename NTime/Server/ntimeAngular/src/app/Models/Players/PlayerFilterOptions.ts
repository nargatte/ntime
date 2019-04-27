import { PlayerSort } from '../Enums/PlayerSort';
import { Distance } from '../Distance';
import { AgeCategory } from '../AgeCategory';
import { Subcategory } from '../Subcategory';

export class PlayerFilterOptions {
  PlayerSort: PlayerSort = PlayerSort.ByLastName;
  DescendingSort = false;
  ExtraDataSortIndex: number;
  Query?: string;
  IsMale?: boolean;
  WithoutStartTime?: boolean;
  Invalid?: boolean;
  CompletedCompetition?: boolean;
  HasVoid?: boolean;
  IsPaidUp?: boolean;
  Distance?: Distance;
  AgeCategory?: AgeCategory;
  Subcategory?: Subcategory;
}
