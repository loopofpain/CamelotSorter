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

  public tableRowsA :SuperTableRow[] = []

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
      let superTableColumn : SuperTableColumn=  {
        columnName: columnName,
        order: ++index
      };

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

      for(let column of this.tableColumns) {
        row.columns.forEach(sourceColumn => {
          if(sourceColumn.columnName == column.columnName){
            const newRowColumn: SuperTableRowColumn = {
              columnName: sourceColumn.columnName,
              value: sourceColumn.value
            }

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
    this.tableRowsA = this.getRows();
  }

  public ngOnInit(): void {
    this.tableColumns = this.getColumns();
    this.tableRowsA = this.getRows();
  }
}
