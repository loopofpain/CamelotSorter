import { SuperTableRow } from "./super-table-row";

export class SuperTableRowColumn {
    public columnName!:string;
    public value?: string;
    public parent: SuperTableRow;

    constructor(parent:SuperTableRow,columnName: string,value?: string) {
      this.columnName = columnName;
      this.value = value;
      this.parent = parent;
    }


    public get cssId(): string {
      let lower = this.columnName.toLowerCase().replace(' ','_');

      return `table-row-${this.parent.index}-column-${lower}`;
    }
}
