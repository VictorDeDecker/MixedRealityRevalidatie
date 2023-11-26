import { Component } from '@angular/core';
import { UnityService } from '../unity.service';
import { ParameterChangeRequest } from '../ParameterChangeRequest';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  speed:number = 2;
  objectsAmount:number = 30;
  timeInSeconds:number = 60;
  response?:string;

  constructor(private unityService:UnityService){
    this.testConnection()
  }

  async testConnection(){
    (await this.unityService.testConnection()).subscribe(value => console.log(value.message));
  }

  async applyAmountOfSeconds() {
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "objectSpawner",
      parameter: "timeInSeconds",
      value: this.timeInSeconds
    };

    (await this.unityService.sendToUnity(parameterChangeRequest)).subscribe(value => this.response = value.message);
  }

  async applyAmountOfObjects() {
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "objectSpawner",
      parameter: "amountOfObjects",
      value: this.objectsAmount
    };

    (await this.unityService.sendToUnity(parameterChangeRequest)).subscribe(value => this.response = value.message);
  }

  async applySpeed() {
    let parameterChangeRequest:ParameterChangeRequest = {
      script: "touchObject",
      parameter: "speed",
      value: this.speed
    };

    (await this.unityService.sendToUnity(parameterChangeRequest)).subscribe(value => this.response = value.message);
  }
}
