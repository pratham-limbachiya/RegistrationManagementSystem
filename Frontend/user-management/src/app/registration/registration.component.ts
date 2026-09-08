import { HttpClient } from '@angular/common/http';
import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-registration',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './registration.component.html',
  styleUrl: './registration.component.css'
})
export class RegistrationComponent {
  selectedId = signal(0);
  states = signal<any[]>([]);
  cities = signal<any[]>([]);
  selectedFiles = signal<File[]>([]);
  previewUrl = signal<string | null>(null);
  previewFileName = signal('');
  previewFileType = signal('');
  showPreview = signal(false);
  showPassword = signal(false);
  hobbies = [
    'Cricket',
    'Football',
    'Reading',
    'Traveling',
    'Music'
  ];
  user = {
    id: 0,
    name: '',
    username: '',
    password: '',
    dateOfBirth: '',
    gender: '',
    hobbies: [] as string[],
    address: '',
    stateId: 0,
    cityId: 0,
    pincode: ''
  };

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit() {
    this.getStates();
    const registration = history.state?.user;
    if (registration) {
      this.selectedId.set(registration.RegistrationId ?? 0);
      this.user = {
        id: registration.RegistrationId ?? 0,
        name: registration.Name ?? '',
        username: registration.Username ?? '',
        password: '',
        dateOfBirth: registration.DateOfBirth
          ? registration.DateOfBirth.substring(0, 10)
          : '',
        gender: registration.Gender ?? '',
        hobbies: registration.Hobbies
          ? registration.Hobbies.split(',')
          : [],
        address: registration.Address ?? '',
        stateId: registration.StateId ?? 0,
        cityId: registration.CityId ?? 0,
        pincode: registration.Pincode ?? ''
      };
      // console.log('Registration Form Data:', this.user);
    }
  }

  getStates() {
    this.http.get<any>('https://localhost:7172/api/Registration/GetStates').subscribe({
      next: (response) => {
        this.states.set(response.result[0]);
      },
      error: (error) => {
        // console.error('Error loading states:', error);
      }
    });
  }

  onHobbyChange(hobby: string, event: Event) {
    const checkbox = event.target as HTMLInputElement;
    if (checkbox.checked) {
      if (!this.user.hobbies.includes(hobby)) {
        this.user.hobbies.push(hobby);
      }
    } else {
      this.user.hobbies =
        this.user.hobbies.filter(x => x !== hobby);
    }
  }
  onStateChange() {
    const stateId = this.user.stateId;
    this.user.cityId = 0;
    this.cities.set([]);
    if (stateId === 0) {
      return;
    }
    this.getCities(stateId);
  }

  getCities(stateId: number) {
    this.http.get<any>(`https://localhost:7172/api/Registration/GetCities/${stateId}`).subscribe({
      next: (response) => {
        this.cities.set(response.result[0]);
      },
      error: (error) => {
        // console.error('Error loading cities:', error);
        this.cities.set([]);
      }
    });
  }

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (!input.files || input.files.length === 0) {
      return;
    }
    const files = Array.from(input.files);
    const invalidFiles = files.filter(file => file.type !== 'image/jpeg' && file.type !== 'image/png');
    if (invalidFiles.length > 0) {
      alert('Only JPG and PNG files are allowed.');
      return;
    }
    this.selectedFiles.update(existingFiles => [...existingFiles, ...files]);
  }
  togglePassword() {
    this.showPassword.update(value => !value);
  }

  previewFile(file: File) {
    const url = URL.createObjectURL(file);
    this.previewUrl.set(url);
    this.previewFileName.set(file.name);
    this.previewFileType.set(file.type);
    this.showPreview.set(true);
  }
  closePreview() {

    const url = this.previewUrl();
    this.previewUrl.set(null);
    this.previewFileName.set('');
    this.previewFileType.set('');
    this.showPreview.set(false);
  }
  submitForm(event: Event) {
    event.preventDefault();
    if (
      !this.user.name.trim() ||
      !this.user.username.trim() ||
      !this.user.dateOfBirth ||
      !this.user.gender ||
      this.user.hobbies.length === 0 ||
      !this.user.address.trim() ||
      this.user.stateId === 0 ||
      this.user.cityId === 0 ||
      !this.user.pincode.trim()
    ) {
      alert('Please fill in all required fields.');
      return;
    }

    if (this.selectedId() === 0 && !this.user.password) {
      alert('Password is required.');
      return;
    }
    if (this.selectedId() === 0 && this.selectedFiles().length === 0) {
      alert('Please select a file.');
      return;
    }
    const formData = new FormData();
    formData.append('Id', this.selectedId().toString());
    formData.append('Name', this.user.name);
    formData.append('Username', this.user.username);
    formData.append('Password', this.user.password);
    formData.append('DateOfBirth', this.user.dateOfBirth);
    formData.append('Gender', this.user.gender);
    formData.append('Hobbies', this.user.hobbies.join(','));
    formData.append('Address', this.user.address);
    formData.append('StateId', this.user.stateId.toString());
    formData.append('CityId', this.user.cityId.toString());
    formData.append('Pincode', this.user.pincode);
    const Files = this.selectedFiles();
    Files.forEach(Files => {
      formData.append('Files', Files, Files.name);
    });

    this.http.post<any>('https://localhost:7172/api/Registration/SaveOrUpdate', formData).subscribe({
      next: (response) => {
        if (this.selectedId() === 0) {
          // Save
          alert('Registration saved successfully.');
          this.resetForm();
          this.router.navigate(['/login']);
        } else {
          // Update
          alert('Registration updated successfully.');
          sessionStorage.setItem('loginResponse', JSON.stringify(response));
          this.resetForm();
          this.router.navigate(['/dashboard']);
        }
      },
      error: (error) => {
        const validationErrors = error?.error?.errors;
        if (validationErrors) {
          const messages: string[] = [];
          Object.keys(validationErrors).forEach(key => {
            const fieldErrors = validationErrors[key];
            if (Array.isArray(fieldErrors)) {
              fieldErrors.forEach(message => {
                messages.push(message);
              });
            } else {
              messages.push(fieldErrors);
            }
          });
          alert(messages.join('\n'));
        } else {
          alert(
            error.error
          );
        }
      }
    });
  }
  resetForm() {
    this.user = {
      id: 0,
      name: '',
      username: '',
      password: '',
      dateOfBirth: '',
      gender: '',
      hobbies: [],
      address: '',
      stateId: 0,
      cityId: 0,
      pincode: ''
    };
    this.selectedFiles.set([]);
    this.cities.set([]);
    this.selectedId.set(0);
    this.previewUrl.set(null);
    this.previewFileName.set('');
    this.previewFileType.set('');
    this.showPreview.set(false);
    const fileInput = document.getElementById(
      'documents'
    ) as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
  }

}
