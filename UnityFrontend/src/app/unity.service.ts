import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UnityResponse } from './UnityResponse';
import { ParameterChangeRequest } from './ParameterChangeRequest';
import { SceneChange } from './SceneChange';

const url:string = "http://localhost:8085"

@Injectable({
  providedIn: 'root'
})
export class UnityService {

  constructor(private http: HttpClient) { }

  async testConnection() {
    return this.http.get<UnityResponse>(url + "/test");
  }

  async sendParameterToUnity(parameterChangeRequest:ParameterChangeRequest){
    return this.http.post<UnityResponse>(url + "/updateParameter", parameterChangeRequest);
  }

  async sendLevelToUnity(SceneChange:SceneChange){
    return this.http.post<UnityResponse>(url + "/changeScene", SceneChange);
  }
}
