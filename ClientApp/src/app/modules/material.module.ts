import { NgModule } from '@angular/core';
import { MatButtonModule, MatInputModule, MatDialogModule, MatCheckboxModule, MatDividerModule, MatTooltipModule, MatGridListModule, MatIconModule, MatCardModule, MatFormFieldModule, MatListModule, MatTableModule, MatSortModule } from '@angular/material'

const material = [
  MatButtonModule,
  MatInputModule,
  MatDialogModule,
  MatCheckboxModule,
  MatDividerModule,
  MatTooltipModule,
  MatGridListModule,
  MatIconModule,
  MatCardModule,
  MatFormFieldModule,
  MatListModule,
  MatTableModule,
  MatSortModule
]

@NgModule({
  imports: [material],
  exports: [material]
})
export class MaterialModule { }
