import { BasicPlayerArguments } from './BasicPlayerArguments';
import { PlayerBase } from './PlayerBase';
import { IPlayerListView } from './IPlayerListView';
import { ExtraColumnValue } from '../ExtraColumns/ExtraColumnValue';

export class PlayerListView extends PlayerBase implements IPlayerListView {
  public StartNumber: number;
  public StartTime: number;
  public FullCategory: string;
  public ExtraData: string;
  public IsPaidUp: boolean;
  public CompetitionId: number;
  public ExtraColumnValues: ExtraColumnValue[];

  constructor(
    args?: BasicPlayerArguments,
    extraColumnValues?: ExtraColumnValue[]
  ) {
    super();
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
    if (extraColumnValues) {
      this.ExtraColumnValues = extraColumnValues;
    }
  }

  copyDataFromDto(dto: IPlayerListView): PlayerListView {
    super.copyDataFromDto(dto);
    this.StartNumber = dto.StartNumber;
    this.StartTime = dto.StartTime;
    this.FullCategory = dto.FullCategory;
    this.ExtraData = dto.ExtraData;
    this.IsPaidUp = dto.IsPaidUp;
    this.CompetitionId = dto.CompetitionId;
    this.ExtraColumnValues = dto.ExtraColumnValues.map(columnValueDto =>
      new ExtraColumnValue().copyDataFromDto(columnValueDto)
    );
    return this;
  }
}
