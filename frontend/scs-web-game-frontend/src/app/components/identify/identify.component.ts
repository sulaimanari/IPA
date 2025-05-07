import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { PlayerDto, PlayersService } from '../../core/api/apiServices';
import { IdentifyService } from '../../services/identify.service';
import { firstValueFrom } from 'rxjs/internal/firstValueFrom';

@Component({
  selector: 'app-identify',
  templateUrl: './identify.component.html',
  styleUrl: './identify.component.css'
})
export class IdentifyComponent {
  public errorMessage: string = '';
  constructor(private router: Router, private playerService: PlayersService, private identify: IdentifyService) { }

  

 createPlayer(email: string) {
    console.log('Creating player with email:', email); 
    firstValueFrom(this.playerService.getOrCreatePlayer(email))
      .then((response: PlayerDto) => {
        console.log('Player created:', response); 
        if (response && response.playerEmail) {
          this.errorMessage = '';
          this.goToHome(response.playerId, response.playerEmail, response.playerName);
        }
      })
      .catch((error) => {
        this.errorMessage = 'Failed to create a new player.';
        console.error('Failed to create a new player:', error); 
      });
  }

 
  goToHome(playerId?: string, playerEmail?: string, playerName?: string) {
    console.log('Setting player data in IdentifyService:', { playerId, playerEmail, playerName });
    this.identify.setPlayerData(playerId!, playerEmail!, playerName!); 
    this.router.navigate(['/home']).catch((error) => {
      console.error('Navigation error:', error);
    });
  }
}
