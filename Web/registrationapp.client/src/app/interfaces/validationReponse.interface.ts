export interface ValidationError {
  PropertyName: string;
  ErrorMessage: string;
}

export interface ValidationResponse {
  Message: string;
  Errors: ValidationError[];
}
