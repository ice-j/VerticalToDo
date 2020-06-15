export class AuthenticationService {
  static isLoggedIn(): boolean {
    const token = localStorage.getItem("token");
    return token != null && token.length > 0;
  }

  static logIn(token: string) {
    localStorage.setItem("token", "Bearer " + token);
  }

  static logOut() {
    localStorage.removeItem("token");
  }

  static getToken() {
    return localStorage.getItem("token");
  }
}
