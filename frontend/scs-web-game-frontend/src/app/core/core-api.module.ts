import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { API_BASE_URL, EmployeesService, GamesService, PlayersService } from './api/apiServices';
import { HttpClientModule } from '@angular/common/http';



@NgModule({
  declarations: [],
  imports: [
    HttpClientModule
  ],
  providers: [
    CommonModule,
    PlayersService,
    GamesService,
    EmployeesService,
    { provide: API_BASE_URL, useValue: 'https://localhost:7167' }
  ]
})
export class CoreApiModule { }
