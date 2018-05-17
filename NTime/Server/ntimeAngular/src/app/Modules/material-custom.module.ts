import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FlexLayoutModule } from '@angular/flex-layout';
import {
  MatButtonModule, MatToolbarModule, MatIconModule,
  MatTabsModule, MatTableModule, MatPaginatorModule,
  MatCardModule, MatFormFieldModule, MatInputModule,
  MatSelectModule, MatDatepickerModule, MatCheckboxModule,
  // MatDialogModule,
  MatPaginatorIntl,
} from '@angular/material';
import {MatDialogModule} from '@angular/material/dialog';
import { MatMomentDateModule } from '@angular/material-moment-adapter';
import { PolishPaginatorIntl } from '../Helpers/polish-paginator-intl';
import { SelectionModel } from '@angular/cdk/collections';
import { MatSidenavModule } from '@angular/material/sidenav';

@NgModule({
  imports: [
    CommonModule, FlexLayoutModule,
    MatToolbarModule, MatButtonModule, MatTabsModule,
    MatIconModule, MatTableModule, MatPaginatorModule,
    MatCardModule, MatFormFieldModule, MatInputModule,
    MatSelectModule, MatDatepickerModule, MatMomentDateModule,
    MatCheckboxModule, MatDialogModule, MatSidenavModule
  ],
  exports: [
    FlexLayoutModule,
    MatToolbarModule, MatButtonModule, MatTabsModule,
    MatIconModule, MatTableModule, MatPaginatorModule,
    MatCardModule, MatFormFieldModule, MatInputModule,
    MatSelectModule, MatDatepickerModule, MatMomentDateModule,
    MatCheckboxModule, MatDialogModule, MatSidenavModule
  ],
  providers: [
    { provide: MatPaginatorIntl, useClass: PolishPaginatorIntl },
  ],
  declarations: []
})
export class MaterialCustomModule { }
