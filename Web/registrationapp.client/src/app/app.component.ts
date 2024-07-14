import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { RegistrationStep1Component } from './registration-step1/registration-step1.component';
import { RegistrationStep2Component } from './registration-step2/registration-step2.component';
import { User } from './interfaces/user.interface';
import { ValidationResponse } from './interfaces/validationReponse.interface';
import { ToastrService } from 'ngx-toastr';
import { ApiConfigService } from './services/apiconfigservice';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RegistrationStep1Component, RegistrationStep2Component, FormsModule, CommonModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  currentStep: number = 1;
  user: User = {
    email: '',
    password: '',
    confirmPassword: '',
    isAgree: false,
    provinceId: 1
  };

  isSubmitted: boolean = false;

  constructor(private http: HttpClient, private toastr: ToastrService) { }

  ngOnInit(): void {
    // Здесь можно выполнить начальную инициализацию, если требуется
  }

  goToStep(step: number): void {
    this.currentStep = step;
  }

  onUserUpdate(updatedUser: User): void {
    this.user = { ...updatedUser };
  }

  validatePasswords(): boolean {
    if (this.user.password !== this.user.confirmPassword) {
      this.toastr.error('Пароли не совпадают!', 'Ошибка');
      return false;
    }
    return true;
  }

  onSubmit(): void {
    if (!this.validatePasswords()) {
      return;
    }

    this.http.post(`${ApiConfigService.getApiUrl()}/Users/register`, this.user)
      .subscribe(
        response => {
          this.toastr.success('Регистрация прошла успешно!', 'Успех');
          this.isSubmitted = true;
        },
        error => {
          this.toastr.error('Ошибка регистрации. Попробуйте позже. Ошибка:' + error.error, 'Ошибка');
        }
      );
  }
}
