export class Distance {
    Id: number;
    Name: string;
    Length: number;
    DistanceTypeEnum: DistanceTypeEnum;
    LapsCount: number;
    TimeLimit: number;
}

export enum DistanceTypeEnum {
    DeterminedDistance,
    DeterminedLaps,
    LimitedTime
}
