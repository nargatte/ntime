import { PlayerSort } from '../Enums/PlayerSort';

export class PlayerFilterOptions {
  PlayerSort: PlayerSort = PlayerSort.ByLastName;
  DescendingSort = false;
  ExtraDataSortIndex: number;
  Query?: string;
  Men?: boolean;
  WithoutStartTime?: boolean;
  Invalid?: boolean;
  CompletedCompetition?: boolean;
  HasVoid?: boolean;
  Distance?: number;
  AgeCategory?: number;
  Subcategory?: number;
}
