import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GameDto, GamesService, HighScoreDto, PlayerDto } from '../../core/api/apiServices';
import { firstValueFrom } from 'rxjs/internal/firstValueFrom';

@Component({
  selector: 'app-high-score',
  templateUrl: './high-score.component.html',
  styleUrl: './high-score.component.css'
})
export class HighScoreComponent {
  public listHighScore: HighScoreDto[] = [];
  public game?: GameDto;

  constructor(
    private router: Router,
    private highScore: GamesService,
    private gameId: ActivatedRoute,
  ) { }
  
  ngOnInit(): void {
    this.getGameIdAndLoadScore();
    this.loadHighScoreList();
  }

  getGameIdAndLoadScore() {
    this.gameId.queryParams.subscribe(params => {
      const gameId = params['gameId'];
      console.log('Game ID received:', gameId);
      if (gameId) {
        this.loadLastGameScore(gameId);
      }
    });
  }
 
  loadHighScoreList() {
    firstValueFrom(this.highScore.highScores()).then(topScore => {
      console.log('loading top 5 high score succeeded' + topScore);
      this.listHighScore = topScore;
    }).catch(error => {
      console.log('high score failed', error)
      alert('high score failed' + error);
    });
  }

  loadLastGameScore(gameId: string) {
    this.highScore.getHighestScoreOfPlayer(gameId).subscribe(GameScore => {
      this.game = { gameId: GameScore.gameId, playerId: GameScore.playerId, score: GameScore.score };
      console.log('Last game score:', this.game);
    }, error => {
      console.error('Failed to load last game score:', error);
    });
  }

  goToHome() {
    this.router.navigate(['/home']).catch((error) => {
      console.error('Navigation error:', error);
    });
  }
}
