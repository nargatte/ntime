import { Injectable } from '@angular/core';

@Injectable()
export class MessageService {
    messages: string[] = [];
    errors: string[] = [];

    constructor() { }

    public addLog(message: string) {
        console.log(message);
        this.messages.push(message);
    }

    public clearMessages(): void {
        this.messages = [];
    }

    public addError(error: string) {
        console.error(error);
        this.errors.push(error);
    }

    public clearErrors(): void {
        this.errors = [];
    }
}
