import { Component } from '@angular/core';
import { TimerService } from '../../services/timer.service';

@Component({
  selector: 'app-timer',
  templateUrl: './timer.component.html',
  styleUrl: './timer.component.css'
})
export class TimerComponent {
  constructor(public timerService: TimerService) { }

  ngOnInit(): void {
    this.timerService.startTimer(); 
  }

  pauseTimer(): void {
    this.timerService.pauseTimer();
  }
}
