export class PlayerFilterOptions {
    PlayerSort: PlayerSort = PlayerSort.ByFirstName;
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

  export enum PlayerSort {
    ByLastName,
    ByFirstName,
    ByTeam,
    ByStartNumber,
    ByStartTime,
    ByFullCategory,
    ByBirthDate,
    ByRank
  }
