class PlayerBase implements IDtoBase<PlayerBaseDto> {
  constructor(
    public Id: number,
    public FirstName: string,
    public LastName: string,
    public IsMale: boolean,
    public City: string,
    public Team: string
  ) {}
  CopyDataFromDto(dto: PlayerBaseDto): void {
    this.Id = dto.Id;
    this.FirstName = dto.FirstName;
    this.LastName = dto.LastName;
    this.IsMale = dto.IsMale;
    this.City = dto.City;
    this.Team = dto.Team;
  }
}
