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
  spaceBetweenCircles:number = 4;
  InfiniteSpawnWaitTime:number = 2;
  movement:boolean = false;
  ducking:boolean = false;
  obstacles:boolean = false;
  response?:string;
  changeParameterResponse?:string;
  allowRedFish:boolean = true;
  redFish:number = 0;
  allowPinkFish:boolean = true;
  pinkFish:number = 0;
  allowGreenFish: boolean = true;
  greenFish:number = 0;
  allowYellowFish:boolean = true;
  yellowFish:number = 0;
  hand:string = "both";

  constructor(private unityService:UnityService){
  }

  async applyAmountOfSeconds() {
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "objectSpawner",
      parameter: "LevelLengthInSec",
      value: (this.timeInSeconds + (this.InfiniteSpawnWaitTime * this.height)/2)
    };
    this.unityService.updateParameters(parameterChangeRequest);
  }

  async applyHeight() {
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "objectSpawner",
      parameter: "Height",
      value: this.height
    };
    this.unityService.updateParameters(parameterChangeRequest);
  }

  async applySpeed() {
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "touchObject",
      parameter: "speed",
      value: this.speed
    };
    this.unityService.updateParameters(parameterChangeRequest);
  }

  async applySpawnRadius(){
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "objectSpawner",
      parameter: "Radius",
      value: this.SpawnRadius
    };
    this.unityService.updateParameters(parameterChangeRequest);
  }

  async applyCircleWidth(){
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "objectSpawner",
      parameter: "ShoulderWidth",
      value: this.spaceBetweenCircles
    };
    this.unityService.updateParameters(parameterChangeRequest);
  }

  async applyInfiniteSpawnWaitTime(){
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "objectSpawner",
      parameter: "WaitBetweenSpawns",
      value: this.InfiniteSpawnWaitTime
    };
    this.unityService.updateParameters(parameterChangeRequest);
  }

  async checkDucking(){
    if(this.ducking){
      let parameterChangeRequest:ParameterChangeRequest = {
        script: "objectSpawner",
        parameter: "ducking",
        value: 1
      };
      this.unityService.updateParameters(parameterChangeRequest);
    } else{
      let parameterChangeRequest:ParameterChangeRequest = {
        script: "objectSpawner",
        parameter: "ducking",
        value: 0
      };
      this.unityService.updateParameters(parameterChangeRequest);
    }
  }

  async checkMovement(){
    if(this.movement){
      let parameterChangeRequest:ParameterChangeRequest = {
        script: "objectSpawner",
        parameter: "movement",
        value: 1
      };
      this.unityService.updateParameters(parameterChangeRequest);
    } else{
      let parameterChangeRequest:ParameterChangeRequest = {
        script: "objectSpawner",
        parameter: "movement",
        value: 0
      };
      this.unityService.updateParameters(parameterChangeRequest);
    }
  }

  async checkObstacles(){
    if(this.obstacles){
      let parameterChangeRequest:ParameterChangeRequest = {
        script: "objectSpawner",
        parameter: "obstacles",
        value: 1
      };
      this.unityService.updateParameters(parameterChangeRequest);
    } else{
      let parameterChangeRequest:ParameterChangeRequest = {
        script: "objectSpawner",
        parameter: "obstacles",
        value: 0
      };
      this.unityService.updateParameters(parameterChangeRequest);
    }
  }

  async changeLevel(level:string){
    let changeLevel: SceneChange = {
      destinationScene: level
    };
    this.unityService.updateScene(changeLevel);
  }

  async veryEasy(){
    this.speed = 2;
    this.timeInSeconds = 180;
    this.spaceBetweenCircles = 4
    this.SpawnRadius = 2;
    this.InfiniteSpawnWaitTime = 2;
    this.obstacles = false;
    this.ducking = false;
    this.movement = false;
    this.applySpeed();
    this.applyAmountOfSeconds();
    this.applyCircleWidth();
    this.applySpawnRadius();
    this.applyInfiniteSpawnWaitTime();
    this.checkObstacles();
    this.checkDucking();
    this.checkMovement();
  }

  async easy(){
    this.speed = 3;
    this.timeInSeconds = 150;
    this.spaceBetweenCircles = 4
    this.SpawnRadius = 2;
    this.InfiniteSpawnWaitTime = 2;
    this.obstacles = false;
    this.ducking = true;
    this.movement = true;
    this.applySpeed();
    this.applyAmountOfSeconds();
    this.applyCircleWidth();
    this.applySpawnRadius();
    this.applyInfiniteSpawnWaitTime();
    this.checkObstacles();
    this.checkDucking();
    this.checkMovement();
  }
  async normal(){
    this.speed = 3;
    this.timeInSeconds = 150;
    this.spaceBetweenCircles = 4
    this.SpawnRadius = 2;
    this.InfiniteSpawnWaitTime = 2;
    this.obstacles = false;
    this.ducking = true;
    this.movement = true;
    this.applySpeed();
    this.applyAmountOfSeconds();
    this.applyCircleWidth();
    this.applySpawnRadius();
    this.applyInfiniteSpawnWaitTime();
    this.checkObstacles();
    this.checkDucking();
    this.checkMovement();
  }
  async hard(){
    this.speed = 4;
    this.timeInSeconds = 150;
    this.spaceBetweenCircles = 4
    this.SpawnRadius = 2;
    this.InfiniteSpawnWaitTime = 1;
    this.obstacles = true;
    this.ducking = true;
    this.movement = true;
    this.applySpeed();
    this.applyAmountOfSeconds();
    this.applyCircleWidth();
    this.applySpawnRadius();
    this.applyInfiniteSpawnWaitTime();
    this.checkObstacles();
    this.checkDucking();
    this.checkMovement();
  }
  async veryHard(){
    this.speed = 5;
    this.timeInSeconds = 150;
    this.spaceBetweenCircles = 5
    this.SpawnRadius = 3;
    this.InfiniteSpawnWaitTime = 1;
    this.obstacles = true;
    this.ducking = true;
    this.movement = true;
    this.applySpeed();
    this.applyAmountOfSeconds();
    this.applyCircleWidth();
    this.applySpawnRadius();
    this.applyInfiniteSpawnWaitTime();
    this.checkObstacles();
    this.checkDucking();
    this.checkMovement();
  }

  async applyRedFish(){
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "objectSpawner",
      parameter: "redFishAmount",
      value: this.redFish
    };
    this.unityService.updateParameters(parameterChangeRequest);
  }

  async applyPinkFish(){
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "objectSpawner",
      parameter: "pinkFishAmount",
      value: this.pinkFish
    };
    this.unityService.updateParameters(parameterChangeRequest);
  }

  async applyGreenFish(){
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "objectSpawner",
      parameter: "greenFishAmount",
      value: this.greenFish
    };
    this.unityService.updateParameters(parameterChangeRequest);
  }

  async applyYellowFish(){
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "objectSpawner",
      parameter: "yellowFishAmount",
      value: this.yellowFish
    };
    this.unityService.updateParameters(parameterChangeRequest);
  }

  async checkRedFish(){
    if(this.allowRedFish){
      let parameterChangeRequest:ParameterChangeRequest = {
        script: "objectSpawner",
        parameter: "allowRedFish",
        value: 0
      };
      this.unityService.updateParameters(parameterChangeRequest);
    }
  }

  async checkPinkFish(){
    if(this.allowPinkFish){
      let parameterChangeRequest:ParameterChangeRequest = {
        script: "objectSpawner",
        parameter: "allowPinkFish",
        value: 0
      };
      this.unityService.updateParameters(parameterChangeRequest);
    }
  }

  async checkGreenFish(){
    if(this.allowGreenFish){
      let parameterChangeRequest:ParameterChangeRequest = {
        script: "objectSpawner",
        parameter: "allowGreenFish",
        value: 0
      };
      this.unityService.updateParameters(parameterChangeRequest);
    }
  }

  async checkYellowFish(){
    if(this.allowYellowFish){
      let parameterChangeRequest:ParameterChangeRequest = {
        script: "objectSpawner",
        parameter: "allowYellowFish",
        value: 0
      };
      this.unityService.updateParameters(parameterChangeRequest);
    }
  }

  async updateHand(hand:string){
    this.hand = hand;
    var value = 0;
    if(this.hand=="both"){
      value = 0
    } else if(this.hand=="right"){
      value = 1
    } else if(this.hand=="left"){
      value = -1;
    }
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "handSelector",
      parameter: "hand",
      value: value
    };
    this.unityService.updateParameters(parameterChangeRequest);
  }
}
