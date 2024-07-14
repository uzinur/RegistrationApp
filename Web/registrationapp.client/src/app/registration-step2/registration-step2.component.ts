import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiConfigService } from '../services/apiconfigservice';

interface User {
  email: string;
  password: string;
  confirmPassword: string;
  isAgree: boolean;
  provinceId: number;
}

@Component({
  selector: 'app-registration-step2',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './registration-step2.component.html',
  styleUrls: ['./registration-step2.component.css']
})
export class RegistrationStep2Component implements OnInit {
  @Input() user!: User;
  @Input() isSubmitted!: boolean;
  @Output() userUpdate = new EventEmitter<User>();
  @Output() submitRegistration = new EventEmitter<void>();

  registrationForm: FormGroup;
  countries: any[] = [];
  provinces: any[] = [];
  
  constructor(private fb: FormBuilder, private http: HttpClient, private toastr: ToastrService) {
    this.registrationForm = this.fb.group({
      selectedCountry: ['', Validators.required],
      selectedProvince: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadCountries();
  }

  loadCountries(): void {
    this.http.get(`${ApiConfigService.getApiUrl()}/countries/countries`)
      .subscribe({
        next: (data: any) => this.countries = data, //TODO: use something else to use strong typing
        error: () => this.toastr.error('Ошибка загрузки списка стран.', 'Ошибка')
      });
  }

  loadProvinces(countryId: string): void {
    this.http.get(`${ApiConfigService.getApiUrl()}/countries/provinces?countryId=${countryId}`)
      .subscribe({
        next: (data: any) => this.provinces = data, //TODO: use something else to use strong typing
        error: () => this.toastr.error('Ошибка загрузки списка провинций.', 'Ошибка')
      });
  }

  onCountryChange(): void {
    this.loadProvinces(this.registrationForm.get('selectedCountry')?.value);
  }

  markAllAsTouched(): void {
    Object.keys(this.registrationForm.controls).forEach(field => {
      const control = this.registrationForm.get(field);
      control?.markAsTouched({ onlySelf: true });
    });
  }

  onSubmit(): void {
    this.markAllAsTouched();

    if (this.registrationForm.invalid) {
      return;
    }

    const selectedProvinceId = this.provinces.find(p => p.name === this.registrationForm.get('selectedProvince')?.value)?.id;
    this.user.provinceId = selectedProvinceId;
    this.userUpdate.emit(this.user);
    this.submitRegistration.emit();
  }
}
