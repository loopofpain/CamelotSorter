export class SuperTableColumn {
    public columnName!:string;
    public order: number = 0;
    public type!: string;

    public get cssId(): string {
      let lower = this.columnName.toLowerCase().replace(' ','_');

      return `table-column-${lower}`;
    }
}
