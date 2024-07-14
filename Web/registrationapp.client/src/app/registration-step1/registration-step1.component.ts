import { Component, EventEmitter, Output } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators, FormsModule, ValidatorFn, AbstractControl, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { CustomValidators } from '../validations/CustomValidators';

interface User {
  email: string;
  password: string;
  confirmPassword: string;
  isAgree: boolean;
  provinceId: number;
}

@Component({
  selector: 'app-registration-step1',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule
  ],
  templateUrl: './registration-step1.component.html',
  styleUrls: ['./registration-step1.component.css']
})
export class RegistrationStep1Component {
  @Output() userUpdate = new EventEmitter<User>();
  @Output() nextStep = new EventEmitter<void>();

  registrationForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private toastr: ToastrService
  ) {
    this.registrationForm = this.fb.group({
      email: ['', [Validators.required, Validators.email], [CustomValidators.emailExists(this.http)]],
      password: ['', [Validators.required, Validators.minLength(6),  CustomValidators.passwordIsWeak()]],
      confirmPassword: ['', [Validators.required]],
      isAgree: [false, [Validators.requiredTrue]]
    }, { validators: this.passwordMatchValidator });
  }

  passwordMatchValidator: ValidatorFn = (form: AbstractControl): ValidationErrors | null => {
    const password = form.get('password');
    const confirmPassword = form.get('confirmPassword');
    if (!password || !confirmPassword) {
      return null;
    }
    return password.value !== confirmPassword.value ? { mismatch: true } : null;
  }

  markAllAsTouched() {
    this.registrationForm.markAllAsTouched();
  }

  onNext() {
    this.markAllAsTouched();

    if (this.registrationForm.invalid) {
      return;
    }

    const user: User = {
      email: this.registrationForm.get('email')?.value,
      password: this.registrationForm.get('password')?.value,
      confirmPassword: this.registrationForm.get('confirmPassword')?.value,
      isAgree: this.registrationForm.get('isAgree')?.value,
      provinceId: 0
    };

    this.userUpdate.emit(user);
    this.nextStep.emit();
  }
}
