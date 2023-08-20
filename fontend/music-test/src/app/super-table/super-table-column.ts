import { ISuperTableColumn } from "./super-table-column.interface";

export class SuperTableColumn implements ISuperTableColumn {
    public columnName!:string;
    public propertyName!: string;
    public order: number = 0;
    public type!: string;

    public get cssId(): string {
      let lower = this.columnName?.toLowerCase()?.replace(' ','_');

      return `table-column-${lower}`;
    }
}
