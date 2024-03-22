import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserToken } from '../shared/UserToken';
import { UserLogin } from '../auth/auth.component';
import { catchError, retry, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl: string = 'https://localhost:44347/User';
  constructor(private http: HttpClient) { }

  loginUser(loginform: UserLogin){
    return this.http.post<UserLogin>('https://localhost:44347/Auth/Login', loginform).pipe(retry(3), catchError(err => this.handleError(err)));
  }

  getUser(param: number){
    return this.http.get(this.baseUrl + '/GetSingleUser/'+ param);
  }

  private handleError(value: Response | any){
    let errorMessage = value.toString();
    let response = value as Response;
    if (response){
      errorMessage = `${response.status}: ${response.statusText}\n${response.toString()}`;
    }
    if(value.error){
      errorMessage = value;
    }
    return throwError(() => errorMessage);
  }

}
