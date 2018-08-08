import { AgeCategory } from '../AgeCategory';
import { Distance } from '../Distance';
import { Subcategory } from '../Subcategory';
import { AgeCategoryDistance } from '../AgeCategoryDistance';
import { Competition } from './Competition';

export class CompetitionWithDetails extends Competition {
    AgeCategories: AgeCategory[];
    Distances: Distance[];
    Subcategories: Subcategory[];
    AgeCategoryDistances: AgeCategoryDistance[];
    constructor(public Id: number, public City: string, public EventDate: Date,
        public SignUpEndDate: Date
    ) {
        super(Id, City, EventDate, SignUpEndDate);
    }

    public static convertDates(competition: CompetitionWithDetails): CompetitionWithDetails {
        competition.EventDate = new Date(competition.EventDate);
        competition.SignUpEndDate = new Date(competition.SignUpEndDate);
        return competition;
    }
}
