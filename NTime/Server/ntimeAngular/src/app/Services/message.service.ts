import { Injectable, isDevMode } from '@angular/core';

@Injectable()
export class MessageService {
    private messages: string[] = [];
    private errors: string[] = [];
    private objects: any[] = [];

    constructor() { }

    public addLog(message: string) {
        if (isDevMode()) {
            console.log(message);
        }
        this.messages.push(message);
    }

    public clearMessages(): void {
        this.messages = [];
    }

    public addError(error: string) {
        if (isDevMode()) {
            console.error(error);
        }
        this.errors.push(error);
    }

    public clearErrors(): void {
        this.errors = [];
    }

    public addObject(item: any) {
        if (isDevMode()) {
            console.log(item);
        }
        this.objects.push(item);
    }

    public clearObjects() {
        this.objects = [];
    }
}
