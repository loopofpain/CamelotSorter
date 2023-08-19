import { Component } from '@angular/core';
import { SuperTableRow } from './super-table/super-table-row';
import { SuperTableRowColumn } from './super-table/super-table-row-column';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'music-test';

  myRows: SuperTableRow[] = []

  private get rows(): SuperTableRow[] {

    const rows: SuperTableRow[] = []

    const row1 = new SuperTableRow();
    row1.index=1;
    row1.columns=[
      new SuperTableRowColumn(row1,'Title','Darude - Sandstorm',typeof("")),
      new SuperTableRowColumn(row1,'Alt Key','A1',typeof("")),
      new SuperTableRowColumn(row1,'Is Fixed','True',typeof(""))

    ];

    const row2 = new SuperTableRow();
    row2.index=2;
    row2.columns=[
      new SuperTableRowColumn(row2, 'Title','Sido - Arschficksong',typeof("")),
      new SuperTableRowColumn(row2, 'Alt Key','A2',typeof("")),
      new SuperTableRowColumn(row2, 'Is Fixed','True',typeof(""))

    ];

    const row3 = new SuperTableRow();
    row3.index=3;
    row3.columns=[
      new SuperTableRowColumn(row3, 'Title','SSIO - NULLKOMMAEINS',typeof("")),
      new SuperTableRowColumn(row3, 'Alt Key','B9',typeof("")),
      new SuperTableRowColumn(row3, 'Is Fixed','False',typeof(""))

    ];

    rows.push(row1)
    rows.push(row2)
    rows.push(row3)

    return rows;
  }

  constructor() {
    this.myRows = this.rows
  }
}
