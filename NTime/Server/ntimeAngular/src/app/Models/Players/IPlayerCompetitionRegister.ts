interface IPlayerCompetitionRegister extends IPlayerListView {
  Email: string;
  BirthDate: Date;
  PhoneNumber: string;
  SubcategoryId: number;
  DistanceId: number;
  AgeCategoryId: number;
  ReCaptchaToken: string;
}
