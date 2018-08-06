import { Competition } from '../Competition';
import { PlayerListView } from '../Players/PlayerListView';

export class ColumnDefinition {
  columnDef: string;
  header: string;
  cell;

  constructor(columnDef: string, header: string) {
    this.columnDef = columnDef;
    this.header = header;
    this.cell = (player: PlayerListView) => `${player.FirstName}`;
  }
}
