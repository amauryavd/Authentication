import { Routes } from '@angular/router';
import { AuthComponent } from './auth/auth.component';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './shared/auth.guard';

export const routes: Routes = [
    { path: '',  component: AuthComponent},
    { path: 'home', component: HomeComponent, canActivate: [AuthGuard]}
];

