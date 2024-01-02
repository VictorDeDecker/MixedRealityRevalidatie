import { Injectable } from '@angular/core';
import { ParameterChangeRequest } from './ParameterChangeRequest';
import { SceneChange } from './SceneChange';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';


@Injectable({
  providedIn: 'root'
})
export class UnityService {
  private hubConnection: HubConnection;

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('http://45.93.139.33:32825/gameHub')
      .build();

      this.startConnection();
  }

  startConnection() {
    this.hubConnection.start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err));
  }

  updateParameters(parameters: ParameterChangeRequest) {
    this.hubConnection.invoke('UpdateParameters', parameters)
      .catch(err => console.error(err));
  }

  updateScene(scene:SceneChange) {
    this.hubConnection.invoke('UpdateScene', scene)
      .catch(err => console.log(err));
  }
}
