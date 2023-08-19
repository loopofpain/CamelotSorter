import { SuperTableRow } from "./super-table-row";

export class SuperTableRowColumn {
  public columnName!: string;
  public inputValue?: any;
  public parent: SuperTableRow;
  private typeFromValue?: string;
  public propertyName: string;

  constructor(parent: SuperTableRow, propertyName: string, columnName: string, inputValue?: any, typeFromValue?: string) {
    this.propertyName = propertyName;
    this.columnName = columnName;
    this.inputValue = inputValue;
    this.parent = parent;
    this.typeFromValue = typeFromValue;
  }

  public get value(): any {
    return this.inputValue;
  }

  public get type(): string {
    if (this.typeFromValue) {
      return this.typeFromValue;
    }

    return "string";
  }

  public onChange($event: any) {
    if (this.type === 'boolean') {
      this.inputValue = !(this.inputValue as boolean)
    } else {
      throw Error('Undefined')
    }
  }

  public get cssId(): string {
    let lower = this.columnName?.toLowerCase()?.replace(' ', '_');

    return `table-row-${this.parent.index}-column-${lower}`;
  }
}
