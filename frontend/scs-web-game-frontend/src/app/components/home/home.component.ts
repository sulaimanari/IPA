import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { IdentifyService } from '../../services/identify.service';
import { PlayerDto } from '../../core/api/apiServices';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  public PlayerData?: PlayerDto;
  constructor(private router: Router, private identify: IdentifyService) { }

  ngOnInit(): void {
    this.loadPlayer();
  }

  loadPlayer() {
    const playerId = this.identify.playerId;
    const playerEmail = this.identify.playerEmail;
    const playerName = this.identify.playerName;

    if (playerId && playerEmail && playerName) {
      console.log('Player ID:', playerId, 'Player Email:', playerEmail, 'Player Name:', playerName);
      this.PlayerData = { playerId: playerId, playerEmail: playerEmail, playerName: playerName };
    } else {
      console.error('Player data is missing. Redirecting to IdentifyComponent.');
      this.router.navigate(['/identify']).catch((error) => {
        console.error('Navigation error:', error);
      });
    }
  }


  goToPlay() {
    if (this.PlayerData) {
      this.router.navigate(['/play']).catch((error) => {
        console.error('Navigation error:', error);
      });
    } else {
      console.error('Player data is missing. Redirecting to IdentifyComponent.');
      this.router.navigate(['/identify']).catch((error) => {
        console.error('Navigation error:', error);
      });
    }
  }
}
