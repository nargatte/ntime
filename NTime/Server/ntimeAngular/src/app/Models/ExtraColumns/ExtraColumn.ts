import { PlayerListView } from '../Players/PlayerListView';

export class ExtraColumn implements IExtraColumn {
  public Id: number;
  public CompetitionId: number;
  public Title: string;
  public IsRequired: boolean;
  public IsDisplayedInPublicList: boolean;
  public IsDisplayedInPublicDetails: boolean;
  public Type: string;
  public SortIndex: number;
  public MultiValueCount: number;
  public MinCharactersValidation: number;
  public MaxCharactersValidation: number;

  public getCellValue = (player: PlayerListView) => {
    const columnValue = player.ExtraColumnValues.find(
      value => value.ColumnId === this.Id
    );
    return columnValue ? columnValue.CustomValue : null;
  }

  copyDataFromDto(dto: IExtraColumn): ExtraColumn {
    this.Id = dto.Id;
    this.CompetitionId = dto.CompetitionId;
    this.Title = dto.Title;
    this.IsRequired = dto.IsRequired;
    this.IsDisplayedInPublicList = dto.IsDisplayedInPublicList;
    this.IsDisplayedInPublicDetails = dto.IsDisplayedInPublicDetails;
    this.Type = dto.Type;
    this.SortIndex = dto.SortIndex;
    this.MultiValueCount = dto.MultiValueCount;
    this.MinCharactersValidation = dto.MinCharactersValidation;
    this.MaxCharactersValidation = dto.MaxCharactersValidation;
    return this;
  }
}
