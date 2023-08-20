import { SuperTableRowColumn } from "./super-table-row-column";
import { ISuperTableRowColumn } from "./super-table-row-column.interface";

export class SuperTableRow {
    public columns: ISuperTableRowColumn[] = []
    public index: number=0;


    public get cssId(): string {
      return `table-row-${this.index}`;
    }
}
