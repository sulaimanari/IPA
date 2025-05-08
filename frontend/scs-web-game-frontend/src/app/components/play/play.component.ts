import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AddScoreDto, EmployeeNameDto, EmployeesService, GameDto, GamesService, PlayerDto, RandomEmployeeAndNameListDto } from '../../core/api/apiServices';
import { TimerService } from '../../services/timer.service';
import { IdentifyService } from '../../services/identify.service';
import { firstValueFrom } from 'rxjs/internal/firstValueFrom';
import { catchError, map, of } from 'rxjs';

@Component({
  selector: 'app-play',
  templateUrl: './play.component.html',
  styleUrl: './play.component.css'
})
export class PlayComponent {
  public PlayerData?: PlayerDto; 
  randomEmployee?: RandomEmployeeAndNameListDto; 
  currentPhoto: string = ''; 
  isClickable: boolean = true; 
  private isGameCreated: boolean = false;  
  public game?: GameDto;
  randomNameList: EmployeeNameDto[] = []; 
  addPoint: AddScoreDto | undefined; 
  highlight: { [key: string]: 'correct' | 'incorrect' | null } = {}; 


  constructor(
    private router: Router,
    private employeeService: EmployeesService,
    private gameService: GamesService,
    private timerService: TimerService,
    private identifyService: IdentifyService,
  ) {}

  ngOnDestroy(): void {
    this.timerService.resetTimer();
  }

  ngOnInit(): void {
    this.loadPlayer(); 
    this.CreateGame();
  }

  loadPlayer() {
    console.log('Loading player data from IdentifyService:', {
      playerId: this.identifyService.playerId,
      playerEmail: this.identifyService.playerEmail,
      playerName: this.identifyService.playerName,
    });

    const playerId = this.identifyService.playerId;
    const playerEmail = this.identifyService.playerEmail;
    const playerName = this.identifyService.playerName;

    if (playerId && playerEmail && playerName) {
      this.PlayerData = { playerId, playerEmail, playerName };
      console.log('Player data loaded:', this.PlayerData);
    } else {
      console.error('Player data is missing. Redirecting to IdentifyComponent.');
      this.router.navigate(['/identify']);
    }
  }

  CreateGame() {
    if (!this.isGameCreated && this.PlayerData) {
      this.isGameCreated = true;
      console.log('Creating game for player:', this.PlayerData.playerId);

      firstValueFrom(this.gameService.createGame(this.PlayerData.playerId)).then(CreateGame => {
        this.game = { gameId: CreateGame.gameId, playerId: CreateGame.playerId, score: CreateGame.score };
        console.log('Game created successfully:', this.game);
        this.timerService.startTimer(this.game.gameId!);
        this.loadNextEmployeeFoto();
      }).catch(error => {
        console.error('Error creating game:', error);
      });
    } else {
      console.error('Game creation skipped. Player data or game state is invalid.');
    }
  }

  loadNextEmployeeFoto() {
    this.employeeService.randomNameListAndEmployee()
      .pipe(
        map((response: any) => {
          console.log('Raw response from backend:', response);

          if (response && typeof response === 'object') {
            this.randomEmployee = response as RandomEmployeeAndNameListDto;
            this.currentPhoto = response.image ? `data:image/jpeg;base64,${response.image}` : '';
            this.randomNameList = Array.isArray(response.randomNameList) ? response.randomNameList : [];
            console.log('Next employee loaded:', this.randomEmployee);
          } else {
            console.error('Invalid response format:', response);
          }
        }),
        catchError((error) => {
          console.error('Error loading next employee:', error);
          return of(null);
        })
      )
      .subscribe();
  }

  IsSelctedNameCorrect(selectedFirstName: string, selectedLastName: string) {
    if (!this.game || !this.PlayerData || !this.randomEmployee) {
      console.error('Game, PlayerData, or RandomEmployee is not initialized.');
      return;
    }

    if (!this.game.gameId || !this.randomEmployee.employeeId || !selectedFirstName || !selectedLastName) {
      console.error('Invalid parameters for addScore request:', {
        gameId: this.game.gameId,
        employeeId: this.randomEmployee.employeeId,
        selectedFirstName,
        selectedLastName,
      });
      alert('An error occurred. Please check your input.');
      this.isClickable = true; 
      return;
    }

    this.isClickable = false; 
    this.timerService.pauseTimer();
    console.log('Sending addScore request with parameters:', {
      gameId: this.game.gameId,
      employeeId: this.randomEmployee.employeeId,
      selectedFirstName,
      selectedLastName,
    });

    this.gameService
      .addScore(
        this.game.gameId,
        this.randomEmployee.employeeId,
        selectedFirstName,
        selectedLastName
      )
      .pipe(
        map((response: AddScoreDto) => {
          if (response.success) {
            console.log('Correct selection! Score updated:', response.game?.score);
            this.game = response.game;
            this.highlight = { [`${selectedFirstName} ${selectedLastName}`]: 'correct' };
          } else {
            console.log('Incorrect selection. Correct name is:', response.correctFirstName, response.correctLastName);
            this.highlight = {
              [`${selectedFirstName} ${selectedLastName}`]: 'incorrect',
              [`${response.correctFirstName} ${response.correctLastName}`]: 'correct',
            };
          }

          setTimeout(() => {
            this.highlight = {};
            this.loadNextEmployeeFoto();
            this.isClickable = true;
            this.timerService.startTimer(this.game?.gameId!);
          }, 2000);
        }),
        catchError((error) => {
          console.error('Error checking selected name:', error);
          if (error.status === 400) {
            alert('Invalid request. Please check your input.');
          } else if (error.status === 500) {
            alert('Server error. Please try again later.');
          } else {
            alert('An unexpected error occurred. Please try again.');
          }
          this.isClickable = true; 
          return of(null);
        })
      )
      .subscribe();
  }

}
