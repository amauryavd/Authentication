import { Component, OnInit } from '@angular/core';
import { JwtTokenService } from '../services/jwt-token.service';
import { Router } from '@angular/router';
import { UserToken } from '../shared/UserToken';
import { UserService } from '../services/user.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {

  userDetails: any = []
  user: any;
  
  constructor(private jwtService: JwtTokenService, private route: Router, private userService: UserService, private http: HttpClient){}

  ngOnInit(){
    this.jwtService.getTokenFromLocal();
    this.user = localStorage.getItem("userId");
    this.userService.getUser(this.user).subscribe(data => {
      this.userDetails = data;
      console.log(this.userDetails);
    });
  }

  getUser(){
    
  }

  logout(){
    this.jwtService.removeTokenFromLocal();
    this.route.navigate(['']);
  }
}
