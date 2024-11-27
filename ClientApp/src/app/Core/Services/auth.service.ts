import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { MatSnackBar } from '@angular/material/snack-bar';
import { jwtDecode } from 'jwt-decode';

interface JwtPayload {
    nameid: string;
    email: string;
    role: string;
}

@Injectable({
    providedIn: 'root',
})
export class AuthService {
    private apiUrl = 'http://localhost:5186';
    private accessToken: string | undefined;
    constructor(private http: HttpClient, private snackBar: MatSnackBar) { }

    getCookie(name: string) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) return parts.pop()?.split(';').shift();
        return undefined;
    }

    private decodeToken() {
        this.accessToken = this.getCookie('accessToken') || '';
        if (this.accessToken) {
            try {
                const decodedToken = jwtDecode<JwtPayload>(this.accessToken);
                return decodedToken;
            } catch (error) {
                console.error("Invalid token");
                return null;
            }
        } else {
            console.error("No access token found");
            return null;
        }
    }
    public getUserId(): string | null{
        const decodedToken = this.decodeToken();

        if(decodedToken){
            return decodedToken.nameid
        }
        return null;
    }
    
    public getRole(): string | null {
        const decodedToken = this.decodeToken();

        if (decodedToken && decodedToken.role) {
            return decodedToken.role;
        } else {
            return null;
        }
    }

    public hasRole(userRole: string): boolean {
        const role = this.getRole();
        return role ? role.includes(userRole) : false;
    }

    public isAdmin(): boolean {
        const role = this.getRole();
        return role === 'admin';
    }

    public isManager(): boolean {
        const role = this.getRole();
        return role === 'manager';
    }

    register(firstName: string, email: string, password: string): Observable<any> {
        const registerData = {
            firstName,
            email,
            password
        };
        return this.http.post(`${this.apiUrl}/auth/register`, registerData);
    }

    login(email: string, password: string): Observable<any> {

        const refreshToken = localStorage.getItem('refreshToken') ?? '';

        const headers = new HttpHeaders({
            'Refresh-Token': refreshToken
        });

        const loginData: any = {
            email,
            password,
        };

        if (refreshToken) {
            loginData.refreshToken = refreshToken;
        }
        else {
            loginData.refreshToken = '';
        }

        return this.http.post(`${this.apiUrl}/auth/login`, loginData, { headers });
    }

    isLoggedIn(): boolean {
        return this.getCookie('accessToken') !== undefined;
    }

    logout(): void {
        document.cookie = 'accessToken=; Max-Age=0; path=/';
    }

    refreshTokens(): Observable<any> {
        const refreshToken = localStorage.getItem('refreshToken') ?? '';

        const headers = new HttpHeaders({
            'Refresh-Token': refreshToken
        });

        return this.http.post(`${this.apiUrl}/auth/refresh-tokens`, {}, { headers })
    }

    setTokens(newTokens: any): void {
        localStorage.setItem('refreshToken', newTokens.refreshToken);
        document.cookie = `accessToken=${newTokens.accesToken}; path=/; secure=false; samesite=none;`;
    }
}