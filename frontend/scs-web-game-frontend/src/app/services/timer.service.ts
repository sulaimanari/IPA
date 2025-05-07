import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';

@Injectable({
  providedIn: 'root'
})
export class TimerService {
  private initialTime = 20;
  private timeLeft = this.initialTime;
  private interval: any;

  private timeLeftSubject = new BehaviorSubject<string>(this.initialTime.toFixed(1));
  public timeLeftDisplay$ = this.timeLeftSubject.asObservable();

  constructor(private router: Router) { }
  endGame() {
    this.timeLeft = 15;
    console.log('endGame');
    this.resetTimer();
    this.router.navigate(['/high-score']).then();
  }

  startTimer() {
    this.interval = setInterval(() => {
      if (this.timeLeft > 0) {
        const newTime = parseFloat((this.timeLeft - 0.1).toFixed(1));
        this.timeLeft = newTime;
        this.timeLeftSubject.next(newTime.toFixed(1)); 
      } else {
        this.endGame();
      }
    }, 100);
  }

  pauseTimer() {
    clearInterval(this.interval);
    console.log('Timer has been reset');
  }

  resumeTimer() {
    this.startTimer(); 
    console.log('Timer has been resumed');
  }

  resetTimer() {
    this.pauseTimer(); 
    this.timeLeft = this.initialTime; 
    this.timeLeftSubject.next(this.initialTime.toFixed(1));
    console.log('Timer has been reset');
  }
}
