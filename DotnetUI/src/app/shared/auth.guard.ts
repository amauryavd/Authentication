import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, map, take } from 'rxjs';
import { JwtTokenService } from '../services/jwt-token.service';
// import { NgToastService } from 'ng-angular-popup';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

constructor(private jwtSwrvice: JwtTokenService, private router: Router){}

  canActivate(): Observable<boolean> | boolean{
    if (this.jwtSwrvice.isTokenExist() && !this.jwtSwrvice.isTokenExpired()){
      return true;
    } else{
      // this.toaster.error({detail:"ERROR",summary:'User not logged in!', duration: 5000})
      this.router.navigate(['']);
      return false;
    }
    
  }
  
}
