import { Component } from '@angular/core';
import { SuperTableRow } from './super-table/super-table-row';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'music-test';

  public rows: SuperTableRow[] = [
    {
      index: 1,
      columns: [
        {
          columnName:"Title",
          value:"Darude - Sandstorm"
        },
        {
          columnName:"Alt Key",
          value:"A1"
        },
        {
          columnName:"Is Fixed",
          value:"True"
        }
      ]
    },
    {
      index: 2,
      columns: [
        {
          columnName:"Title",
          value:"Sido - Arschficksong"
        },
        {
          columnName:"Alt Key",
          value:"A2"
        },
        {
          columnName:"Is Fixed",
          value:"False"
        }
      ]
    },
    {
      index: 3,
      columns: [
        {
          columnName:"Title",
          value:"SSIO - NULLKOMMAEINS"
        },
        {
          columnName:"Alt Key",
          value:"B8"
        },
        {
          columnName:"Is Fixed",
          value:"False"
        }
      ]
    }
  ];
}
