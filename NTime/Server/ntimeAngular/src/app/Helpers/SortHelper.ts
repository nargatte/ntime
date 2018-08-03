import { PlayerSort } from '../Models/Enums/PlayerSort';
import { SortDirection } from '../../../node_modules/@angular/material';

export class SortHelper {
    public static getPlayerSort(sortId: string): PlayerSort {
        // 'firstName', 'lastName', 'city', 'team', 'fullCategory', 'isPaidUp'
        switch (sortId) {
            case 'firstName': {
                return PlayerSort.ByFirstName;
            }
            case 'lastName': {
                return PlayerSort.ByLastName;
            }
            case 'team': {
                return PlayerSort.ByTeam;
            }
            case 'fullCategory': {
                return PlayerSort.ByFullCategory;
            }
            default: {
                return PlayerSort.ByLastName;
            }
        }
    }

    public static isSortDescending(sortOrder: SortDirection): boolean {
        switch (sortOrder) {
            case 'asc': return false;
            case 'desc': return true;
            default: return null;
        }
    }
}
