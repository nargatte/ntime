export class PlayerListView extends PlayerBase implements IPlayerListView {
  public StartNumber: number;
  public StartTime: number;
  public FullCategory: string;
  public ExtraData: string;
  public IsPaidUp: boolean;
  public CompetitionId: number;
  public ExtraColumnValues: IExtraColumnValue[];

  constructor(id: number, firstName: string, lastName: string) {
    super();
    this.Id = id;
    this.FirstName = firstName;
    this.LastName = lastName;
  }

  copyDataFromDto(dto: IPlayerListView): void {
    super.copyDataFromDto(dto);
    this.StartNumber = dto.StartNumber;
    this.StartTime = dto.StartTime;
    this.FullCategory = dto.FullCategory;
    this.ExtraData = dto.ExtraData;
    this.IsPaidUp = dto.IsPaidUp;
    this.CompetitionId = dto.CompetitionId;
    this.ExtraColumnValues = dto.ExtraColumnValues;
  }
}
