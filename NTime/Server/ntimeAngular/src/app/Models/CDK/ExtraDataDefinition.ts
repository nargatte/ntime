import { PlayerListView } from '../Players/PlayerListView';

export class ExtraDataDefinition {
  columnDef: string;
  header: string;
  getCellValue: (player: PlayerListView) => string;

  constructor(columnDef: string, header: string, columnIndex: number, delimiter: string) {
    this.columnDef = columnDef;
    this.header = header;
    this.getCellValue = (player: PlayerListView) => player.ExtraData ? `${player.ExtraData.split(delimiter)[columnIndex]}` : '';
  }
}
