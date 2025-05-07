import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TimerComponent } from './components/timer/timer.component';
import { HomeComponent } from './components/home/home.component';
import { HighScoreComponent } from './components/high-score/high-score.component';
import { PlayComponent } from './components/play/play.component';
import { IdentifyComponent } from './components/identify/identify.component';
import { HeaderComponent } from './components/header/header.component';

const routes: Routes = [
  { path: '', redirectTo: 'identify', pathMatch: 'full' },
  { path: 'identify', component: IdentifyComponent },
  { path: 'timer', component: TimerComponent },
  { path: 'home', component: HomeComponent },
  { path: 'header', component: HeaderComponent },
  { path: 'high-score', component: HighScoreComponent },
  { path: 'play', component: PlayComponent },
  { path: '**', redirectTo: 'identify' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
