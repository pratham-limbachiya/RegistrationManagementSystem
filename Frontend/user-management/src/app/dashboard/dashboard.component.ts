import { Component, computed, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  message = signal('');
  userDetails = signal<any[]>([]);
  selectedColumn = signal('All');
  searchText = signal('');
  filterColumns = [
    { label: 'All', value: 'All' },
    { label: 'Name', value: 'Name' },
    { label: 'Username', value: 'Username' },
    { label: 'Gender', value: 'Gender' },
    { label: 'Date of Birth', value: 'DateOfBirth' },
    { label: 'Address', value: 'Address' },
    { label: 'City', value: 'CityName' },
    { label: 'State', value: 'StateName' },
    { label: 'Pincode', value: 'Pincode' }
  ];
  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit() {
    const storedResponse = sessionStorage.getItem('loginResponse');
    if (!storedResponse) {
      this.router.navigate(['/login']);
      return;
    } else {
      const responseee = JSON.parse(storedResponse);
      // // console.log('Dashboard Response:', response);
      // this.userDetails.set(response.result?.[0] || []);
      this.http.get<any>('https://localhost:7172/api/Login/getuserDetails').subscribe({
        next: (response) => {
          this.message.set(responseee.message || '');
          this.userDetails.set(response.result[0]);
        },
        error: (error) => {
          // console.error('Error loading states:', error);
        }
      });

    }
  }

  logout() {
    sessionStorage.removeItem('loginResponse');
    this.router.navigate(['/login']);
  }
  filteredUsers = computed(() => {
    const users = this.userDetails();
    const column = this.selectedColumn();
    const search = this.searchText()
      .trim()
      .toLowerCase();
    if (!search) {
      return users;
    }
    return users.filter(user => {
      // All
      if (column === 'All') {
        return Object.values(user).some(value =>
          String(value ?? '')
            .trim()
            .toLowerCase()
            .includes(search)
        );
      }
      if (column === 'Gender') {
        const gender = String(user.Gender ?? '')
          .trim()
          .toLowerCase();
        return gender.includes(search);
      }
      // Other columns
      const value = String(user[column] ?? '')
        .trim()
        .toLowerCase();
      return value.includes(search);
    });
  });
  onColumnChange(event: Event) {
    const value = (event.target as HTMLSelectElement).value;
    this.selectedColumn.set(value);
  }

  // Search change
  onSearch(event: Event) {
    const value = (event.target as HTMLInputElement).value;
    this.searchText.set(value);
  }

  editUser(user: any) {
    this.router.navigate(['/registration'], {
      state: {
        user: user
      }
    });
    // sessionStorage.removeItem('loginResponse');
  }

  deleteUser(id: number) {
    console.log('Delete User ID:', id);
    const url = `https://localhost:7172/api/Login/DeleteUser/${id}`;
    this.http.post<any>(url, {}).subscribe({
      next: (response) => {
        console.log('Delete Response:', response);
        if (response.statusCode === 200) {
          alert(response.message);
          this.userDetails.update(users =>
            users.filter(user => user.RegistrationId !== id)
          );
          const storedResponse = sessionStorage.getItem('loginResponse');
          if (storedResponse) {
            const loginResponse = JSON.parse(storedResponse);
            loginResponse.result[0] =
              loginResponse.result[0].filter(
                (user: any) => user.RegistrationId !== id
              );
            sessionStorage.setItem('loginResponse', JSON.stringify(loginResponse));
            if (loginResponse.result[0].length === 0) {
              sessionStorage.removeItem('loginResponse');
              this.router.navigate(['/login']);
              return;
            }
          }
        }
        else if (response.statusCode === 404) {
          alert(response.message);
        }
      },
      error: (error) => {
        console.error('Delete API Error:', error);
        alert(
          error?.error?.message ||
          'Invalid username or password.'
        );

      }
    });
  }
}
