import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { SuperTableRow } from './super-table-row';
import { SuperTableColumn } from './super-table-column';
import { SuperTableRowColumn } from './super-table-row-column';

@Component({
  selector: 'app-super-table',
  templateUrl: './super-table.component.html',
  styleUrls: ['./super-table.component.scss']
})
export class SuperTableComponent implements OnInit, OnChanges{

  @Input() public rows: SuperTableRow[] = []

  public tableColumns: SuperTableColumn[] = [];

  public draggingTableColumn?: SuperTableColumn;

  public tableRowsToDisplay :SuperTableRow[] = []

  constructor() {

  }

  public getColumns(): SuperTableColumn[]
  {
    const result: SuperTableColumn[] = [];

    let columnsDictionary: any = { };

    this.rows.forEach(row => {
      row.columns.forEach(column => {
        columnsDictionary[column.columnName]=+1;
      });
    });

    let index=0;

    Object.keys(columnsDictionary).forEach(columnName => {
      let superTableColumn : SuperTableColumn= new SuperTableColumn()
        superTableColumn.columnName= columnName;
        superTableColumn.order= ++index;


      result.push(superTableColumn);
    });

    return result;
  }

  public getRows(): SuperTableRow[] {
    const resultRows: SuperTableRow[]  = [];

    let index=0;

    for (let row of this.rows) {
      const resultRow = new SuperTableRow();
      row.index = ++index;
      resultRow.index = row.index

      for(let column of this.tableColumns) {
        row.columns.forEach(sourceColumn => {
          if(sourceColumn.columnName === column.columnName){
            const newRowColumn: SuperTableRowColumn= new SuperTableRowColumn(resultRow,sourceColumn.columnName,sourceColumn.value);

            resultRow.columns.push(newRowColumn);
          }
        });
      }

      resultRows.push(resultRow);
    }

    return resultRows;
  }

  public ngOnChanges(changes: SimpleChanges): void {
    this.tableColumns = this.getColumns();
    this.tableRowsToDisplay = this.getRows();
  }

  public ngOnInit(): void {
    this.tableColumns = this.getColumns();
    this.tableRowsToDisplay = this.getRows();
  }

  public drop(ev:DragEvent) {
    ev.preventDefault();

    const indexDraggedElement = this.tableColumns.indexOf(this.draggingTableColumn!);
    let indexOfTargetElement = -1;

    for (let index = 0; index < this.tableColumns.length; index++) {
      const element = this.tableColumns[index];
      const currentTarget: HTMLElement | null= ev.currentTarget as HTMLElement;

      if(element.cssId === currentTarget.id){
        indexOfTargetElement = index;
        break;
      }
    }

    const orderOfDraggedElementInArray = this.tableColumns[indexDraggedElement].order;

    this.tableColumns[indexDraggedElement].order = this.tableColumns[indexOfTargetElement].order;
    this.tableColumns[indexOfTargetElement].order = orderOfDraggedElementInArray;

    this.swapElements(this.tableColumns,indexDraggedElement,indexOfTargetElement);

    this.draggingTableColumn = undefined;

    this.tableRowsToDisplay = this.getRows();
    this.rows = this.tableRowsToDisplay;
  }


  public dragOver(event: DragEvent){
    if(this.draggingTableColumn){
      event.preventDefault();
    }
  };

  public dragStart(event: DragEvent, item: SuperTableColumn){
    event!.dataTransfer!.setData('text', item.columnName);
    event!.dataTransfer!.effectAllowed = 'move';
    this.draggingTableColumn = item;
  };

  public dragEnd(event: DragEvent, item: SuperTableColumn){
    this.draggingTableColumn = undefined;
  };


  private swapElements<T>(array: T[], index1: number, index2: number): void {
    [array[index1], array[index2]] = [array[index2], array[index1]];
  }
}
