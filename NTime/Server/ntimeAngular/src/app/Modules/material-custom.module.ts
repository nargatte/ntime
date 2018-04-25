import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  MatButtonModule, MatToolbarModule, MatIconModule,
  MatTabsModule, MatTableModule, MatPaginatorModule,
  MatCardModule, MatFormFieldModule, MatInputModule,
  MatSelectModule, MatDatepickerModule, MatCheckboxModule,
  MatDialogModule,
  MatPaginatorIntl,
} from '@angular/material';
import { MatMomentDateModule } from '@angular/material-moment-adapter';
import { PolishPaginatorIntl } from '../Helpers/PolishPaginatorIntl';
import { SelectionModel } from '@angular/cdk/collections';

@NgModule({
  imports: [
    CommonModule,
    MatToolbarModule, MatButtonModule, MatTabsModule,
    MatIconModule, MatTableModule, MatPaginatorModule,
    MatCardModule, MatFormFieldModule, MatInputModule,
    MatSelectModule, MatDatepickerModule, MatMomentDateModule,
    MatCheckboxModule, MatDialogModule,
  ],
  exports: [
    MatToolbarModule, MatButtonModule, MatTabsModule,
    MatIconModule, MatTableModule, MatPaginatorModule,
    MatCardModule, MatFormFieldModule, MatInputModule,
    MatSelectModule, MatDatepickerModule, MatMomentDateModule,
    MatCheckboxModule, MatDialogModule,
  ],
  providers: [
    { provide: MatPaginatorIntl, useClass: PolishPaginatorIntl },
  ],
  declarations: []
})
export class MaterialCustomModule { }
