import { AbstractControl, ValidationErrors, ValidatorFn, AsyncValidatorFn } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { ApiConfigService } from '../services/apiconfigservice';

export class CustomValidators {
  static passwordIsWeak(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const value = control.value;
      if (!value) {
        return null;
      }

      const hasLetter = /[\p{L}]/u.test(value);
      const hasNumber = /\d/.test(value);

      const passwordValid = hasLetter && hasNumber;

      return !passwordValid ? { passwordIsWeak: true } : null;
    };
  }

  static emailExists(http: HttpClient): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      if (!control.value) {
        return of(null);
      }

      return http.get(`${ApiConfigService.getApiUrl()}/Users/isEmailEsists?email=${control.value}`).pipe(
        map((response: any) => {
          return response ? { emailExists: true } : null;
        }),
        catchError(() => of(null))
      );
    };
  }
}
