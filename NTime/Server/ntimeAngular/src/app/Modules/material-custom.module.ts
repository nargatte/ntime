import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
   MatButtonModule, MatToolbarModule, MatIconModule,
    MatTabsModule, MatTableModule, MatPaginatorModule,
  } from '@angular/material';

@NgModule({
  imports: [
    CommonModule,
    MatToolbarModule, MatButtonModule, MatTabsModule,
     MatIconModule, MatTableModule, MatPaginatorModule,
  ],
  exports: [
    MatToolbarModule, MatButtonModule, MatTabsModule,
    MatIconModule,  MatTableModule, MatPaginatorModule,
  ],
  declarations: []
})
export class MaterialCustomModule { }
