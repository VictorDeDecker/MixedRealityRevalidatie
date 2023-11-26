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
  timeInSeconds:number = 60;
  MaxAmountOfMissingObjects:number = 50;
  InfiniteSpawn:boolean = false;
  Width:number = 5;
  InfiniteSpawnWaitTime:number = 2;
  response?:string;
  changeParameterResponse?:string;

  constructor(private unityService:UnityService){
    setInterval(()=>{
      this.testConnection()
    },2500)
  }

  async testConnection(){
    (await this.unityService.testConnection()).subscribe(value => {
      console.log(value.message)
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
      value: this.timeInSeconds
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

  async applyInfiniteSpawn(){
    let value:number;
    if(this.InfiniteSpawn){
      value = 1;
    } else{
      value = 0;
    }

    let parameterChangeRequest:ParameterChangeRequest = {
      script: "objectSpawner",
      parameter: "InfiniteSpawn",
      value: value
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
}
