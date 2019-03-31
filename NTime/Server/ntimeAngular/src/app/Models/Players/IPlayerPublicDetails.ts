interface IPlayerPublicDetails extends IPlayerListView {
  RegistrationDate: Date;
  LapsCount: number;
  Time: number;
  DistancePlaceNumber: number;
  CategoryPlaceNumber: number;
  CompetitionCompleted: boolean;
  SubcategoryId: number;
  DistanceId: number;
  AgeCategoryId: number;
  PlayerAccountId: number;
}
