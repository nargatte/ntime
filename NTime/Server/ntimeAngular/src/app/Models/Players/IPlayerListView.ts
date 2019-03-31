interface IPlayerListView extends IPlayerBase {
  StartNumber: number;
  StartTime: number;
  FullCategory: string;
  ExtraData: string;
  IsPaidUp: boolean;
  CompetitionId: number;
  ExtraColumnValues: IExtraColumnValue[];
}
