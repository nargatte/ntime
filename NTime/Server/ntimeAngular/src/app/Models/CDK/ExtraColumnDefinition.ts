import { PlayerListView } from '../Players/PlayerListView';

export class ExtraColumnDefinition {
  columnDef: string;
  header: string;
  cell;

  constructor(columnDef: string, header: string, columnIndex: number, delimiter: string) {
    this.columnDef = columnDef;
    this.header = header;
    this.cell = (player: PlayerListView) => `${player.ExtraData.split(delimiter)[columnIndex]}`;
  }
}