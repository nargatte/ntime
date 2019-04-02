import { IPlayerBase } from './IPlayerBase';
import { IExtraColumnValue } from '../ExtraColumns/IExtraColumnValue';

export interface IPlayerListView extends IPlayerBase {
  StartNumber: number;
  StartTime: number;
  FullCategory: string;
  ExtraData: string;
  IsPaidUp: boolean;
  CompetitionId: number;
  ExtraColumnValues: IExtraColumnValue[];
}
