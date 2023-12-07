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
  objectsAmount:number = 30;
  timeInSeconds:number = 30;
  MaxAmountOfMissingObjects:number = 50;
  Width:number = 5;
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
      value: (this.timeInSeconds + (this.InfiniteSpawnWaitTime * this.objectsAmount)/2)
    };

    (await this.unityService.sendParameterToUnity(parameterChangeRequest)).subscribe(value => this.changeParameterResponse = value.message);
  }

  async applyAmountOfObjects() {
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "objectSpawner",
      parameter: "AmountOfSets",
      value: this.objectsAmount
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

  async applyPercentageOfMissingObjects(){
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "objectSpawner",
      parameter: "MaxPercentageOfMissingObjects",
      value: this.MaxAmountOfMissingObjects/100
    };

    (await this.unityService.sendParameterToUnity(parameterChangeRequest)).subscribe(value => this.changeParameterResponse = value.message);
  }

  async applySetWidth(){
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "objectSpawner",
      parameter: "SetWidth",
      value: this.Width
    };

    (await this.unityService.sendParameterToUnity(parameterChangeRequest)).subscribe(value => this.changeParameterResponse = value.message);
  }

  async applyInfiniteSpawnWaitTime(){
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "objectSpawner",
      parameter: "SetWidth",
      value: this.Width
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
    this.objectsAmount = 20;
    this.timeInSeconds = 30;
    this.MaxAmountOfMissingObjects = 65;
    this.InfiniteSpawnWaitTime = 4;
    this.applySpeed();
    this.applyAmountOfObjects();
    this.applyAmountOfSeconds();
    this.applyPercentageOfMissingObjects();
    this.applyInfiniteSpawnWaitTime();
  }

  async easy(){
    this.speed = 3;
    this.objectsAmount = 40;
    this.timeInSeconds = 30;
    this.MaxAmountOfMissingObjects = 60;
    this.InfiniteSpawnWaitTime = 4;
    this.applySpeed();
    this.applyAmountOfObjects();
    this.applyAmountOfSeconds();
    this.applyPercentageOfMissingObjects();
    this.applyInfiniteSpawnWaitTime();
  }
  async normal(){
    this.speed = 4;
    this.objectsAmount = 50;
    this.timeInSeconds = 20;
    this.MaxAmountOfMissingObjects = 50;
    this.InfiniteSpawnWaitTime = 3;
    this.applySpeed();
    this.applyAmountOfObjects();
    this.applyAmountOfSeconds();
    this.applyPercentageOfMissingObjects();
    this.applyInfiniteSpawnWaitTime();
  }
  async hard(){
    this.speed = 4;
    this.objectsAmount = 50;
    this.timeInSeconds = 10;
    this.MaxAmountOfMissingObjects = 40;
    this.InfiniteSpawnWaitTime = 2;
    this.applySpeed();
    this.applyAmountOfObjects();
    this.applyAmountOfSeconds();
    this.applyPercentageOfMissingObjects();
    this.applyInfiniteSpawnWaitTime();
  }
  async veryHard(){
    this.speed = 5;
    this.objectsAmount = 50;
    this.timeInSeconds = 1;
    this.MaxAmountOfMissingObjects = 30;
    this.InfiniteSpawnWaitTime = 2;
    this.applySpeed();
    this.applyAmountOfObjects();
    this.applyAmountOfSeconds();
    this.applyPercentageOfMissingObjects();
    this.applyInfiniteSpawnWaitTime();
  }
}
