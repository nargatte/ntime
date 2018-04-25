import { Injectable } from '@angular/core';

@Injectable()
export class MessageService {
    private messages: string[] = [];
    private errors: string[] = [];
    private objects: any[] = [];

    constructor() { }

    public addLog(message: string) {
        // console.log(message);
        this.messages.push(message);
    }

    public clearMessages(): void {
        this.messages = [];
    }

    public addError(error: string) {
        // console.error(error);
        this.errors.push(error);
    }

    public clearErrors(): void {
        this.errors = [];
    }

    public addObject(item: any) {
        // console.log(item);
        this.objects.push(item);
    }

    public clearObjects() {
        this.objects = [];
    }
}
