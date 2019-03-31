interface PlayerCompetitionRegisterDto extends PlayerBaseDto {
    PhoneNumber: string;
    BirthDate: Date;
    ExtraData: string;
    Email: string;
    SubcategoryId: number;
    DistanceId: number;
    AgeCategoryId: number;
    CompetitionId: number;
    ReCaptchaToken: string;
}
