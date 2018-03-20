export class StringHelper {
    public static generatPageRequest(pageSize: number, pageNumber: number) {
        return `?ItemsOnPage=${pageSize}&PageNumber=${pageNumber}`;
    }
}
