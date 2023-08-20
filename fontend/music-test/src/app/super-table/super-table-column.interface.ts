export interface ISuperTableColumn {
    columnName:string;
    propertyName: string;
    order: number;
    type: string;

    get cssId(): string
}
