import { SuperTableRow } from "./super-table-row";

export interface ISuperTableRowColumn {
  columnName: string;
  inputValue: any | undefined;
  parent: SuperTableRow;
  propertyName: string | undefined;

  onChange($event: any): void

  get value(): any | undefined

  get type(): string | undefined


  get cssId(): string
}
