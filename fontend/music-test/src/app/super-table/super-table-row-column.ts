import { SuperTableRow } from "./super-table-row";

export class SuperTableRowColumn {
    public columnName!:string;
    public value?: any;
    public parent: SuperTableRow;
    private typeFromValue?: string;

    constructor(parent:SuperTableRow,columnName: string, value?: any, typeFromValue?: string) {
      this.columnName = columnName;
      this.value = value;
      this.parent = parent;
      this.typeFromValue = typeFromValue;
    }


    public get type(): string {
      if(this.typeFromValue){
        return this.typeFromValue;
      }

      return "string";
    }

    public get cssId(): string {
      let lower = this.columnName.toLowerCase().replace(' ','_');

      return `table-row-${this.parent.index}-column-${lower}`;
    }
}
