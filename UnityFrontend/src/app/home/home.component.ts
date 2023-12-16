import { Component } from '@angular/core';
import { UnityService } from '../unity.service';
import { ParameterChangeRequest } from '../ParameterChangeRequest';
import { SceneChange } from '../SceneChange';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  speed:number = 2;
  height:number = 2;
  timeInSeconds:number = 30;
  SpawnRadius:number = 2;
  spaceBetweenCircles:number = 2;
  InfiniteSpawnWaitTime:number = 2;
  response?:string;
  changeParameterResponse?:string;

  constructor(private unityService:UnityService){
    setInterval(()=>{
      this.testConnection()
    },5000)
  }

  async testConnection(){
    (await this.unityService.testConnection()).subscribe(value => {
      if(value.message = "OK") {
        this.response = "Connected"
      } else {
        this.response = "Disconnected"
      }
    });
  }

  async applyAmountOfSeconds() {
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "objectSpawner",
      parameter: "LevelLengthInSec",
      value: (this.timeInSeconds + (this.InfiniteSpawnWaitTime * this.height)/2)
    };

    (await this.unityService.sendParameterToUnity(parameterChangeRequest)).subscribe(value => this.changeParameterResponse = value.message);
  }

  async applyHeight() {
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "objectSpawner",
      parameter: "Height",
      value: this.height
    };

    (await this.unityService.sendParameterToUnity(parameterChangeRequest)).subscribe(value => this.changeParameterResponse = value.message);
  }

  async applySpeed() {
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "touchObject",
      parameter: "speed",
      value: this.speed
    };

    (await this.unityService.sendParameterToUnity(parameterChangeRequest)).subscribe(value => this.changeParameterResponse = value.message);
  }

  async applySpawnRadius(){
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "objectSpawner",
      parameter: "Radius",
      value: this.SpawnRadius
    };

    (await this.unityService.sendParameterToUnity(parameterChangeRequest)).subscribe(value => this.changeParameterResponse = value.message);
  }

  async applyCircleWidth(){
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "objectSpawner",
      parameter: "ShoulderWidth",
      value: this.spaceBetweenCircles
    };

    (await this.unityService.sendParameterToUnity(parameterChangeRequest)).subscribe(value => this.changeParameterResponse = value.message);
  }

  async applyInfiniteSpawnWaitTime(){
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "objectSpawner",
      parameter: "WaitBetweenSpawns",
      value: this.InfiniteSpawnWaitTime
    };

    (await this.unityService.sendParameterToUnity(parameterChangeRequest)).subscribe(value => this.changeParameterResponse = value.message);
  }

  async changeLevel(level:string){
    let changeLevel: SceneChange = {
      scene: level
    };

    (await this.unityService.sendLevelToUnity(changeLevel)).subscribe(value => this.response = value.message);
  }

  async veryEasy(){
    this.speed = 2;
    this.height = 20;
    this.timeInSeconds = 30;
    this.SpawnRadius = 65;
    this.InfiniteSpawnWaitTime = 4;
    this.applySpeed();
    this.applyHeight();
    this.applyAmountOfSeconds();
    this.applySpawnRadius();
    this.applyInfiniteSpawnWaitTime();
  }

  async easy(){
    this.speed = 3;
    this.height = 40;
    this.timeInSeconds = 30;
    this.SpawnRadius = 60;
    this.InfiniteSpawnWaitTime = 4;
    this.applySpeed();
    this.applyHeight();
    this.applyAmountOfSeconds();
    this.applySpawnRadius();
    this.applyInfiniteSpawnWaitTime();
  }
  async normal(){
    this.speed = 4;
    this.height = 50;
    this.timeInSeconds = 20;
    this.SpawnRadius = 50;
    this.InfiniteSpawnWaitTime = 3;
    this.applySpeed();
    this.applyHeight();
    this.applyAmountOfSeconds();
    this.applySpawnRadius();
    this.applyInfiniteSpawnWaitTime();
  }
  async hard(){
    this.speed = 4;
    this.height = 50;
    this.timeInSeconds = 10;
    this.SpawnRadius = 40;
    this.InfiniteSpawnWaitTime = 2;
    this.applySpeed();
    this.applyHeight();
    this.applyAmountOfSeconds();
    this.applySpawnRadius();
    this.applyInfiniteSpawnWaitTime();
  }
  async veryHard(){
    this.speed = 5;
    this.height = 50;
    this.timeInSeconds = 1;
    this.SpawnRadius = 30;
    this.InfiniteSpawnWaitTime = 2;
    this.applySpeed();
    this.applyHeight();
    this.applyAmountOfSeconds();
    this.applySpawnRadius();
    this.applyInfiniteSpawnWaitTime();
  }
}
