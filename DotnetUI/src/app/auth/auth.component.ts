import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Route, Router } from '@angular/router';
import { JwtTokenService } from '../services/jwt-token.service';
import { NgToastModule, NgToastService } from 'ng-angular-popup';
import { UserService } from '../services/user.service';


@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [CommonModule, HttpClientModule, FormsModule, ReactiveFormsModule, NgToastModule],
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.css'
})
export class AuthComponent {
  title = 'Authenter';
  loginForm: FormGroup;
  registerForm: FormGroup;
  switch = false;
  errorMsgShow = { visibility: "hidden" };
  errorMsg: any;
  isLoading = false;
  loginButton = "login";
  registerButton = "register";
  showLoginPassword =  false;
  showRegisterPassword = false;
  jwt: any;
  decodedJWT: any;

  constructor(private http: HttpClient, private fb: FormBuilder, private route: Router, private jwtService: JwtTokenService, private userService: UserService) {
    this.loginForm = this.fb.group({
      Email: [{value: '', disabled: this.isLoading}, [Validators.required, Validators.email]],
      Password: [{value: '', disabled: this.isLoading}, [Validators.required, Validators.minLength(4)]],
    });
    this.registerForm = this.fb.group({
      FirstName: ['', Validators.required],
      LastName: ['', Validators.required],
      Gender: ['', Validators.required],
      Email: ['', [Validators.required, Validators.email]],
      Password: ['', [Validators.required, Validators.minLength(4)]],
      PasswordConfirm: ['', [Validators.required, Validators.minLength(4)]]
    });

  }

  registerUser(registerform: FormGroup) {
    const body: UserRegister = {
      Email: registerform.get('Email')?.value,
      Password: registerform.get('Password')?.value,
      PasswordConfirm: registerform.get('PasswordConfirm')?.value,
      FirstName: registerform.get('FirstName')?.value,
      LastName: registerform.get('LastName')?.value,
      Gender: registerform.get('Gender')?.value
      // Gender: registerform.value.Gender
    };
    this.isLoading = true;
    this.registerButton = 'Processing';
    this.http.post<UserRegister>('https://localhost:44347/Auth/Register', body).subscribe(res => {
      this.isLoading = false;
      this.registerButton = 'Register';
      this.registerForm = this.fb.group({
        FirstName: '',
        LastName: '',
        Gender: '',
        Email: '',
        Password: '',
        PasswordConfirm: ''
      });
      this.switch = false;
      return alert("Registered");
    }, error => {
      this.isLoading = false;
      this.registerButton = 'Register';
      console.log(error)
      this.errorMsg = error.Message;
      this.errorMsgShow = { visibility: "visible" };
    });
  }

  loginUser(loginform: FormGroup) {
    const body: UserLogin = {
      Email: loginform.get('Email')?.value,
      Password: loginform.get('Password')?.value
    };
    this.isLoading = true;
    this.loginButton = 'Processing';
    this.userService.loginUser(body).subscribe(res => {
      this.isLoading = false;
      this.loginButton = 'Login';
      this.loginForm = this.fb.group({
        Email: '',
        Password: ''
      });
      this.route.navigate(['/home']);
      this.jwt = res;
      this.jwtService.setToken(this.jwt.token);
      this.jwtService.setTokenToLocal();
      
    }, error => {
      this.isLoading = false;
      this.loginButton = 'Login';
      this.errorMsg = error.error;
      this.errorMsgShow = { visibility: "visible" };
    });
  }

  onFocus() {
    this.errorMsgShow = { visibility: "hidden" };
  }

  onShowPassword(item: string) {
    if (item == 'login'){
      this.showLoginPassword = !this.showLoginPassword;
    }
    if (item == 'register'){
      this.showRegisterPassword = !this.showRegisterPassword;
    }
  }

}

export interface UserRegister{
  Email: string;
  Password: string;
  PasswordConfirm: string;
  FirstName: string;
  LastName: string;
  Gender: string;
}

export interface UserLogin{
  Email: string;
  Password: string;
}
