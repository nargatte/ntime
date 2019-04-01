import { PlayerListView } from '../Players/PlayerListView';

export class ExtraDataDefinition {
  columnDef: string;
  header: string;
  getCellValue: (player: PlayerListView) => string = (player: PlayerListView) => 'test1';

  constructor(columnDef: string, header: string, columnIndex: number, delimiter: string) {
    this.columnDef = columnDef;
    this.header = header;
    // this.getCellValue = (player: PlayerListView) => `${player.ExtraData.split(delimiter)[columnIndex]}`;
  }
}
