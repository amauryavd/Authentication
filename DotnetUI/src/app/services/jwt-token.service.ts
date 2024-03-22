import { Injectable } from '@angular/core';
import  { jwtDecode, JwtPayload } from 'jwt-decode';
import { BehaviorSubject } from 'rxjs';
import { UserToken } from '../shared/UserToken';

@Injectable({
  providedIn: 'root'
})
export class JwtTokenService {
  jwtToken!: string;
  decodedToken!: { [key: string]: string; };
  user = new BehaviorSubject<UserToken|null>(null);

  constructor() { }

  setToken(token: string) {
    if (token) {
      this.jwtToken = token;
    }
  }

  decodeToken() {
    if (this.jwtToken) {
    this.decodedToken = jwtDecode(this.jwtToken);
    }
  }

  getDecodeToken() {
    return jwtDecode<JwtPayload>(this.jwtToken);
  }

  getUser() {
    this.decodeToken();
    return this.decodedToken ? this.decodedToken['userId'] : null;
  }

  getExpiryTime() {
    this.decodeToken();
    return this.decodedToken ? this.decodedToken['exp'] : null;
  }

  isTokenExpired(): boolean {
    const expiryTime: any = this.getExpiryTime();
    console.log(new Date(expiryTime*1000))
    if (expiryTime) {
      return ((1000 * expiryTime) - (new Date()).getTime()) < 5000;
    } else {
      return false;
    }
  }

  setTokenToLocal(){
    const userId = this.getUser();
    localStorage.setItem("token", this.jwtToken);
    localStorage.setItem("userId", userId!);
  }

  getTokenFromLocal(){
    return localStorage.getItem("token");
  }

  removeTokenFromLocal(){
    localStorage.removeItem("token");
    localStorage.removeItem("userId");
  }

  isTokenExist(): boolean{
    return localStorage.getItem("token") != null;
  }

}
