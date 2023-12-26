import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginUser } from 'src/app/models/login-user.model';
import { AuthService } from 'src/app/services/auth.service';
import { TokenService } from 'src/app/services/token.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  errors: string[] = [];

  loginForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
  });
 constructor(private authService: AuthService,
  private router: Router){}

  login() {
    if (this.loginForm.invalid) {
      this.errors = [];
      if (this.loginForm.get('email')?.hasError('required')) {
        this.errors.push('Email is required field');
      }
      if (this.loginForm.get('email')?.hasError('email')) {
        this.errors.push('Email field does not follow email pattern');
      }
      if (this.loginForm.get('password')?.hasError('required')) {
        this.errors.push('Password is required field');
      }
      if (this.loginForm.get('password')?.hasError('pattern')) {
        this.errors.push(
          'Minimum length is 8 and it must have at-least 1 special character, 1 number and 1 alphabet are required in password'
        );
      }
      console.error(this.errors);
      return;
    }

    const user: LoginUser = {
      email: this.loginForm.value.email!,
      password: this.loginForm.value.password!,
    };

    this.authService.login(user).subscribe({
      next: (response) => {
        this.errors = [];
        this.authService.storeToken(response.token);
        this.router.navigate(['']);
      },
      error: (err) => {
        this.errors = [...this.errors, err.error.errors];
        console.log(err);
      },
    });
  }
}
