import { PlayerSort } from '../Enums/PlayerSort';

export class PlayerFilterOptions {
  PlayerSort: PlayerSort = PlayerSort.ByLastName;
  DescendingSort = false;
  Query?: string;
  Men?: boolean;
  WithoutStartTime?: boolean;
  Invalid?: boolean;
  CompleatedCompetition?: boolean;
  HasVoid?: boolean;
  Distance?: number;
  AgeCategory?: number;
  ExtraPlayerInfo?: number;
}


