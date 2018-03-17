import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  MatButtonModule, MatToolbarModule, MatIconModule,
  MatTabsModule, MatTableModule, MatPaginatorModule,
  MatCardModule, MatFormFieldModule, MatInputModule,
  MatSelectModule, MatDatepickerModule,
} from '@angular/material';
import {MatMomentDateModule} from '@angular/material-moment-adapter';

@NgModule({
  imports: [
    CommonModule,
    MatToolbarModule, MatButtonModule, MatTabsModule,
    MatIconModule, MatTableModule, MatPaginatorModule,
    MatCardModule, MatFormFieldModule, MatInputModule,
    MatSelectModule, MatDatepickerModule, MatMomentDateModule,
  ],
  exports: [
    MatToolbarModule, MatButtonModule, MatTabsModule,
    MatIconModule, MatTableModule, MatPaginatorModule,
    MatCardModule, MatFormFieldModule, MatInputModule,
    MatSelectModule, MatDatepickerModule, MatMomentDateModule,
  ],
  declarations: []
})
export class MaterialCustomModule { }
