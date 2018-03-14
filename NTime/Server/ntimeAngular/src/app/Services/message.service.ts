import { Injectable } from '@angular/core';

@Injectable()
export class MessageService {
    message: string[] = [];

    constructor() { }

    public addLog(message: string) {
        console.log(message);
        this.message.push(message);
    }

    public clear(): void {
        this.message = [];
    }
}
