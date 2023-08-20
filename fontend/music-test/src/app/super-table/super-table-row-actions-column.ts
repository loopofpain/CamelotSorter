import { SuperTableRow } from "./super-table-row";
import { ISuperTableRowColumn } from "./super-table-row-column.interface";

export class SuperTableRowActionsColumn implements ISuperTableRowColumn{
  public columnName: string = "Actions";
  public inputValue: any | undefined = undefined;
  public parent: SuperTableRow;
  public propertyName: string | undefined = "";

  constructor(parent: SuperTableRow) {
    this.parent = parent;
  }

  public get value(): any | undefined {
    return this.inputValue;
  }

  public get type(): string | undefined {
    return 'actions';
  }

  public onChange($event: any): void {

  }

  public get cssId(): string {
    return `table-row-${this.parent.index}-column-actions`;
  }
}
