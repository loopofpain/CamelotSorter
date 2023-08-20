import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { SuperTableRow } from './super-table-row';
import { SuperTableColumn } from './super-table-column';
import { SuperTableRowColumn } from './super-table-row-column';
import { CdkDragDrop } from "@angular/cdk/drag-drop";
import { ISuperTableColumn } from './super-table-column.interface';

@Component({
  selector: 'app-super-table',
  templateUrl: './super-table.component.html',
  styleUrls: ['./super-table.component.scss']
})
export class SuperTableComponent implements OnInit, OnChanges {

  @Input() public rows: SuperTableRow[] = []

  public tableColumns: ISuperTableColumn[] = [];

  public draggingTableColumn?: ISuperTableColumn;

  public draggingTableRow?: SuperTableRowColumn;

  public tableRowsToDisplay: SuperTableRow[] = []

  @Input() public lockColumnName: string = "Is Fixed"

  constructor() {

  }

  public getColumns(): ISuperTableColumn[] {
    const result: ISuperTableColumn[] = [];

    let columnsDictionary: any = {};

    this.rows.forEach(row => {
      row.columns.forEach(column => {
        if (columnsDictionary[column.columnName] !== undefined) {
          columnsDictionary[column.columnName].index++;
        } else {

          columnsDictionary[column.columnName] = {
            index: 1,
            type: column.type,
            propertyName: column.propertyName
          }
        }
      });
    });

    columnsDictionary['Save'] = {
      index: 1,
      type: 'boolean',
      propertyName: ''
    }

    let index = 0;

    Object.keys(columnsDictionary).forEach(columnName => {
      let superTableColumn: SuperTableColumn = new SuperTableColumn()
      superTableColumn.columnName = columnName;
      superTableColumn.order = ++index;
      superTableColumn.type = columnsDictionary[columnName].type
      superTableColumn.propertyName = columnsDictionary[columnName].propertyName
      result.push(superTableColumn);
    });

    return result;
  }

  public canEdit(row: SuperTableRow): boolean {
    let result = false;

    row.columns.forEach(column => {
      if (column.columnName === 'Save' && column.value === true) {
        result = true;
        return;
      }
    })

    return result && !this.isRowLocked(row);
  }

  public getRows(): SuperTableRow[] {
    const resultRows: SuperTableRow[] = [];

    let index = 0;

    for (let row of this.rows) {
      const resultRow = new SuperTableRow();
      row.index = ++index;
      resultRow.index = row.index

      let hasSave = false;

      for (let column of this.tableColumns) {
        row.columns.forEach(sourceColumn => {
          if (sourceColumn.columnName === column.columnName && sourceColumn.columnName !== 'Save') {
            const newRowColumn: SuperTableRowColumn = new SuperTableRowColumn(resultRow, column.propertyName, sourceColumn.columnName, sourceColumn.inputValue, sourceColumn.type);
            resultRow.columns.push(newRowColumn);
          }

          if (sourceColumn.columnName === "Save") {
            hasSave = true;
          }
        });
      }

      const newRowColumn: SuperTableRowColumn = new SuperTableRowColumn(resultRow, '', 'Save', false, 'boolean');
      resultRow.columns.push(newRowColumn);

      resultRows.push(resultRow);
    }

    return resultRows;
  }

  public addRow(): void {
    let index = this.tableRowsToDisplay[this.tableRowsToDisplay.length - 1].index + 1;

    const resultRow = new SuperTableRow();
    resultRow.index = index

    for (let column of this.tableColumns) {

      let defaultValue: any = '';

      if (column.type === 'string') {

      } else if (column.type === 'boolean') {
        defaultValue = false;
      } else if (column.type === 'number') {
        defaultValue = 0;
      }

      if (column.columnName === "Save") {
        continue
      }

      const newRowColumn: SuperTableRowColumn = new SuperTableRowColumn(resultRow, column.propertyName, column.columnName, defaultValue, column.type);
      resultRow.columns.push(newRowColumn);

    }


    const newRowColumn: SuperTableRowColumn = new SuperTableRowColumn(resultRow, '', 'Save', false, 'boolean');
    resultRow.columns.push(newRowColumn);


    this.tableRowsToDisplay.push(resultRow);

  }


  public isRowLocked(row: SuperTableRow): boolean {
    let hasMatch = false;

    row.columns.forEach(column => {
      if (column.columnName === this.lockColumnName && column.value === true) {
        hasMatch = true;
        return;
      }
    })

    return hasMatch;
  }

  public ngOnChanges(changes: SimpleChanges): void {
    this.tableColumns = this.getColumns();
    this.tableRowsToDisplay = this.getRows();
  }

  public ngOnInit(): void {
    this.tableColumns = this.getColumns();
    this.tableRowsToDisplay = this.getRows();
  }

  public tableColumnOnDrop(ev: DragEvent) {
    ev.preventDefault();

    const indexDraggedElement = this.tableColumns.indexOf(this.draggingTableColumn!);

    if (indexDraggedElement === -1) {
      this.draggingTableColumn = undefined;
      return;
    }

    let indexOfTargetElement = -1;

    for (let index = 0; index < this.tableColumns.length; index++) {
      const element = this.tableColumns[index];
      const currentTarget: HTMLElement | null = ev.currentTarget as HTMLElement;

      if (element.cssId === currentTarget.id) {
        indexOfTargetElement = index;
        break;
      }
    }

    const orderOfDraggedElementInArray = this.tableColumns[indexDraggedElement].order;

    this.tableColumns[indexDraggedElement].order = this.tableColumns[indexOfTargetElement].order;
    this.tableColumns[indexOfTargetElement].order = orderOfDraggedElementInArray;

    this.swapElements(this.tableColumns, indexDraggedElement, indexOfTargetElement);

    ev.dataTransfer?.clearData();

    this.draggingTableColumn = undefined;

    this.rows = this.tableRowsToDisplay;
    this.tableRowsToDisplay = this.getRows();
  }

  public tableColumnOnDragOver(event: DragEvent) {
    if (this.draggingTableColumn) {
      event.preventDefault();
    }
  };

  public tableColumnOnDragStart(event: DragEvent, item: SuperTableColumn) {
    event!.dataTransfer!.setData('text', item.columnName);
    event!.dataTransfer!.effectAllowed = 'move';
    this.draggingTableColumn = item;
  };

  public tableColumnOnDragEnd(event: DragEvent, item: SuperTableColumn) {
    this.draggingTableColumn = undefined;
  };


  public dropSuperTableRowColumn(event: CdkDragDrop<SuperTableRowColumn[]>) {
    let currentIndex = this.tableRowsToDisplay[event.currentIndex].index;

    if (this.isRowLocked(this.tableRowsToDisplay[event.currentIndex]) || this.isRowLocked(this.tableRowsToDisplay[event.previousIndex])) {
      return;
    }

    this.tableRowsToDisplay[event.currentIndex].index = this.tableRowsToDisplay[event.previousIndex].index;
    this.tableRowsToDisplay[event.previousIndex].index = currentIndex;

    this.swapElements(this.tableRowsToDisplay, event.currentIndex, event.previousIndex);

    this.rows = this.tableRowsToDisplay;
    this.tableRowsToDisplay = this.getRows();
  }

  public getResult(): any {
    const result: any[] = [];

    this.tableRowsToDisplay.forEach(row => {
      let newListElement: any = {}

      row.columns.forEach(column => {
        if (column.propertyName !== '') {
          newListElement[column.propertyName] = column.value
        }
      })

      result.push(newListElement)
    });

    console.log(result);

    return result;
  }

  private swapElements<T>(array: T[], index1: number, index2: number): void {
    [array[index1], array[index2]] = [array[index2], array[index1]];
  }

  public getColSpan(): number {
    return this.tableColumns?.length ?? 0
  }

}
