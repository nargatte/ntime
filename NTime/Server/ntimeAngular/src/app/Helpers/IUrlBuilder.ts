export interface IUrlBuilder {
    toString(): string;
    addControllerName(controllerName: string): IUrlBuilder;
    addId(id: number): IUrlBuilder;
    addPageRequest(pageSize: number, pageNumber: number);
}
