import { IPlayerBase } from './IPlayerBase';

export class PlayerBase implements IPlayerBase {
  public Id: number;
  public FirstName: string;
  public LastName: string;
  public IsMale: boolean;
  public City: string;
  public Team: string;

  constructor() {}
  copyDataFromDto(dto: IPlayerBase): PlayerBase {
    this.Id = dto.Id;
    this.FirstName = dto.FirstName;
    this.LastName = dto.LastName;
    this.IsMale = dto.IsMale;
    this.City = dto.City;
    this.Team = dto.Team;
    return this;
  }
}
