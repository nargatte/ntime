import { IUrlBuilder } from './IUrlBuilder';

export class UrlBuilder implements IUrlBuilder {
    private url = '';

    public addControllerName(controllerName: string): IUrlBuilder {
        this.url += '/api';
        this.url += `/${controllerName}`;
        return this;
    }

    public addId(id: number): IUrlBuilder {
        this.url += `/${id}`;
        return this;
    }
    public toString(): string {
        return this.url;
    }

    public addPageRequest(pageSize: number, pageNumber: number) {
        this.url += `?ItemsOnPage=${pageSize}&PageNumber=${pageNumber}`;
        return this;
    }
}
