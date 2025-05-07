import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class IdentifyService {
  private readonly PLAYER_ID_KEY = 'playerId';
  private readonly PLAYER_EMAIL_KEY = 'playerEmail';
  private readonly PLAYER_NAME_KEY = 'playerName';

  public playerId?: string;
  public playerEmail?: string;
  public playerName?: string;

  constructor() {
    this.playerId = localStorage.getItem(this.PLAYER_ID_KEY) || undefined;
    this.playerEmail = localStorage.getItem(this.PLAYER_EMAIL_KEY) || undefined;
    this.playerName = localStorage.getItem(this.PLAYER_NAME_KEY) || undefined;
  }

  setPlayerData(playerId: string, playerEmail: string, playerName: string): void {
    this.playerId = playerId;
    this.playerEmail = playerEmail;
    this.playerName = playerName;

    localStorage.setItem(this.PLAYER_ID_KEY, playerId);
    localStorage.setItem(this.PLAYER_EMAIL_KEY, playerEmail);
    localStorage.setItem(this.PLAYER_NAME_KEY, playerName);
  }

  clearPlayerData(): void {
    this.playerId = undefined;
    this.playerEmail = undefined;
    this.playerName = undefined;

    localStorage.removeItem(this.PLAYER_ID_KEY);
    localStorage.removeItem(this.PLAYER_EMAIL_KEY);
    localStorage.removeItem(this.PLAYER_NAME_KEY);
  }
}
