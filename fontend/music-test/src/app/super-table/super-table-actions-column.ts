import { ISuperTableColumn } from "./super-table-column.interface";

export class SuperTableActionsColumn implements ISuperTableColumn {
  public columnName: string = "Actions";
  public propertyName: string = "";
  public order: number = Number.MAX_VALUE;
  public type: string = 'Actions'

  public get cssId(): string {
    return `table-column-actions`
  }
}
