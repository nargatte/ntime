export class ExtraFieldDefinition {
    placeholder: string;
    fieldName: string;
    fieldIndex: number;
    delimiter: string;

    constructor(fieldName: string, placeholder: string, fieldIndex: number, delimiter: string) {
        this.fieldName = fieldName;
        this.placeholder = placeholder;
        this.fieldIndex = fieldIndex;
        this.delimiter = delimiter;
    }
}
