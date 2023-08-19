import { SuperTableRowColumn } from "./super-table-row-column";

export class SuperTableRow {
    public columns: SuperTableRowColumn[] = []
    public index: number=0;


    public get cssId(): string {
      return `table-row-${this.index}`;
    }
}
