import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { PlayComponent } from './components/play/play.component';
import { TimerComponent } from './components/timer/timer.component';
import { IdentifyComponent } from './components/identify/identify.component';
import { HighScoreComponent } from './components/high-score/high-score.component';
import { CoreApiModule } from './core/core-api.module';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HeaderComponent } from './components/header/header.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    PlayComponent,
    TimerComponent,
    IdentifyComponent,
    HighScoreComponent,
    HeaderComponent,
  ],
  imports: [
    CoreApiModule,
    CommonModule,
    FormsModule, 
    BrowserModule,
    AppRoutingModule,
    NgOptimizedImage,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
