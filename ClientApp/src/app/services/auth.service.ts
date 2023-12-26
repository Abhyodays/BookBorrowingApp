import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject, Observable } from 'rxjs';
import { LoginUser } from '../models/login-user.model';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private url = 'https://localhost:5001/account';
  private tokenChange = new BehaviorSubject<string | null>(this.getToken());
  constructor(private http: HttpClient, private router: Router,
    private tokenService: TokenService) {}

  login(user: LoginUser): Observable<any> {
    return this.http.post<any>(`${this.url}/login`, user);
  }

  logout() {
    this.removeToken();
    this.router.navigate(['login']);
  }

  storeToken(token: string) {
    localStorage.setItem('token', token);
    this.tokenChange.next(token);
  }

  removeToken() {
    localStorage.removeItem('token');
    this.tokenChange.next(null);
  }

  getToken(): string {
    return localStorage.getItem('token')!;
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    return !!token && !this.isTokenExpired();
  }

  isTokenExpired(): boolean {
    const jwtHelper = new JwtHelperService();
    return jwtHelper.isTokenExpired(this.getToken());
  }

  public decodeToken(token: string | null) {
    if (!token) {
      return '';
    }
    const jwtHelper = new JwtHelperService();
    return jwtHelper.decodeToken(token);
  }

  getTokenChange(): BehaviorSubject<string | null> {
    return this.tokenChange;
  }
}
