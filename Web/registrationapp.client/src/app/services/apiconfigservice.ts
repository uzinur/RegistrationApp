export class ApiConfigService {
  private static readonly apiUrl = 'https://localhost:7153/api';

  public static getApiUrl(): string {
    return this.apiUrl;
  }
}
