import { IUrlBuilder } from './IUrlBuilder';

export class UrlBuilder implements IUrlBuilder {
    private url = '';

    public toString(): string {
        return this.url;
    }

    public addControllerName(controllerName: string): IUrlBuilder {
        this.url += '/api';
        this.url += `/${controllerName}`;
        return this;
    }

    public addId(id: number): IUrlBuilder {
        this.url += `/${id}`;
        return this;
    }

    public addPageRequest(pageSize: number, pageNumber: number): IUrlBuilder {
        this.url += `?ItemsOnPage=${pageSize}&PageNumber=${pageNumber}`;
        return this;
    }

    public addCustomUrlPart(urlPart: string): IUrlBuilder {
        this.url += urlPart;
        return this;
    }

}
