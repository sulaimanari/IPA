import { Component } from '@angular/core';
import { PlayerDto } from '../../core/api/apiServices';
import { IdentifyService } from '../../services/identify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
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
    }
  }
}
