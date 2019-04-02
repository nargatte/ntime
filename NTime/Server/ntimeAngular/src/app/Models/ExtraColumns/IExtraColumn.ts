export interface IExtraColumn extends IDtoBase<IExtraColumn> {
  CompetitionId: number;
  Title: string;
  IsRequired: boolean;
  IsDisplayedInPublicList: boolean;
  IsDisplayedInPublicDetails: boolean;
  Type: string;
  SortIndex: number;
  MultiValueCount: number;
  MinCharactersValidation: number;
  MaxCharactersValidation: number;
}
