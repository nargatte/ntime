class ExtraColumnValue implements IExtraColumnValue {
  public Id: number;
  public PlayerId: number;
  public ColumnId: number;
  public CustomValue: string;
  public LookupId: number;

  copyDataFromDto(dto: IExtraColumnValue): void {
    this.Id = dto.Id;
    this.PlayerId = dto.PlayerId;
    this.ColumnId = dto.ColumnId;
    this.CustomValue = dto.CustomValue;
    this.LookupId = dto.LookupId;
  }
}
