import { HttpClient } from '@angular/common/http';
import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  showPassword = signal(false);
  login = {
    username: '',
    password: ''
  };
  constructor(private http: HttpClient, private router: Router) { }

  togglePassword() {
    this.showPassword.update(value => !value);
  }
  loginUser() {
    if (
      !this.login.username.trim() ||
      !this.login.password
    ) {
      alert('Please enter username and password.');
      return;
    }
    const request = {
      Username: this.login.username.trim(),
      Password: this.login.password
    };
    this.http.post('https://localhost:7172/api/Login/Login', request).subscribe({
      next: (response: any) => {
        // console.log('Login Response:', response);
        if (response?.statusCode === 200) {
          sessionStorage.setItem('loginResponse', JSON.stringify(response));
          alert(response.message);
          this.router.navigate(['/dashboard']);
        } else {
          alert(response?.message || 'Invalid username or password.');
        }
      },
      error: (error) => {
        // console.error('Login Error:', error);
        alert(
          error?.error?.message ||
          'Invalid username or password.'
        );
      }
    });
  }
}
