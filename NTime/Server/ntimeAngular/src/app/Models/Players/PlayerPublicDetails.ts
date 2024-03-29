import { PlayerListView } from './PlayerListView';
import { BasicPlayerArguments } from './BasicPlayerArguments';
import { IPlayerPublicDetails } from './IPlayerPublicDetails';
import { ExtraColumnValue } from '../ExtraColumns/ExtraColumnValue';

export class PlayerPublicDetails extends PlayerListView implements IPlayerPublicDetails {
  public RegistrationDate: Date;
  public LapsCount: number;
  public Time: number;
  public DistancePlaceNumber: number;
  public CategoryPlaceNumber: number;
  public CompetitionCompleted: boolean;
  public SubcategoryId: number;
  public DistanceId: number;
  public AgeCategoryId: number;
  public PlayerAccountId: number;

  constructor(args?: BasicPlayerArguments,
    extraColumnValues?: ExtraColumnValue[]) {
    super(args, extraColumnValues);
  }

  copyDataFromDto(dto: IPlayerPublicDetails): PlayerPublicDetails {
    super.copyDataFromDto(dto);
    this.RegistrationDate = dto.RegistrationDate;
    this.LapsCount = dto.LapsCount;
    this.Time = dto.Time;
    this.DistancePlaceNumber = dto.DistancePlaceNumber;
    this.CategoryPlaceNumber = dto.CategoryPlaceNumber;
    this.CompetitionCompleted = dto.CompetitionCompleted;
    this.SubcategoryId = dto.SubcategoryId;
    this.DistanceId = dto.DistanceId;
    this.AgeCategoryId = dto.AgeCategoryId;
    this.PlayerAccountId = dto.PlayerAccountId;
    return this;
  }
}
