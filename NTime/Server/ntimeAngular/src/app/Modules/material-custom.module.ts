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
  MatSortModule,
  MatExpansionModule,
  MatProgressSpinnerModule,
  MatTooltipModule,
} from '@angular/material';
import { MatDialogModule } from '@angular/material/dialog';
import { MatMomentDateModule } from '@angular/material-moment-adapter';
import { PolishPaginatorIntl } from '../Helpers/polish-paginator-intl';
import { MatSidenavModule } from '@angular/material/sidenav';
import { CdkTableModule } from '../../../node_modules/@angular/cdk/table';

@NgModule({
  imports: [
    CommonModule, FlexLayoutModule,
    MatButtonModule, MatCardModule, MatCheckboxModule, MatExpansionModule,
    MatMomentDateModule, MatDatepickerModule, MatDialogModule,
    MatFormFieldModule, MatIconModule, MatInputModule, MatPaginatorModule,
    MatProgressSpinnerModule, MatSelectModule, MatSidenavModule, MatSortModule,
    MatTableModule, MatTabsModule, MatToolbarModule, CdkTableModule, MatTooltipModule
  ],
  exports: [
    FlexLayoutModule,
    MatButtonModule, MatCardModule, MatCheckboxModule, MatExpansionModule,
    MatMomentDateModule, MatDatepickerModule, MatDialogModule,
    MatFormFieldModule, MatIconModule, MatInputModule, MatPaginatorModule,
    MatProgressSpinnerModule, MatSelectModule, MatSidenavModule, MatSortModule,
    MatTableModule, MatTabsModule, MatToolbarModule, CdkTableModule, MatTooltipModule
  ],
  providers: [
    { provide: MatPaginatorIntl, useClass: PolishPaginatorIntl },
  ],
  declarations: []
})
export class MaterialCustomModule { }
