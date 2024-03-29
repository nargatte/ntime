import { IExtraColumnValue } from './IExtraColumnValue';

export class ExtraColumnValue implements IExtraColumnValue {
  public Id: number;
  public PlayerId: number;
  public ColumnId: number;
  public CustomValue: string;
  public LookupId: number;

  constructor(columnId?: number) {
    if (columnId) {
      this.ColumnId = columnId;
    }
  }

  copyDataFromDto(dto: IExtraColumnValue): ExtraColumnValue {
    this.Id = dto.Id;
    this.PlayerId = dto.PlayerId;
    this.ColumnId = dto.ColumnId;
    this.CustomValue = dto.CustomValue;
    this.LookupId = dto.LookupId;
    return this;
  }
}
