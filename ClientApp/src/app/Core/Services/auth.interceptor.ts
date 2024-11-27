import { Injectable } from "@angular/core";
import { AuthService } from "./auth.service";
import { Router } from '@angular/router';
import Cookies from 'js-cookie';
import {
    HttpEvent,
    HttpInterceptor,
    HttpHandler,
    HttpRequest,
    HttpErrorResponse,
} from "@angular/common/http";
import { catchError, Observable, switchMap, throwError } from "rxjs";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(private authService: AuthService, private router: Router) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const accessToken = Cookies.get('accessToken');

        const mainRequest = accessToken
            ? req.clone({
                setHeaders: {
                    Authorization: `Bearer ${accessToken}`
                }
            })
            : req;

        return next.handle(mainRequest).pipe(
            catchError((error: HttpErrorResponse) => {
                if (error.status === 401) {
                    if (error.error === "Invalid access token") {
                        return this.authService.refreshTokens().pipe(
                            switchMap((newTokens) => {
                                this.authService.setTokens(newTokens);
                                const cloneRequest = req.clone({
                                    setHeaders: {
                                        Authorization: `Bearer ${newTokens.accesToken}`
                                    }
                                });
                                return next.handle(cloneRequest);
                            }),
                            catchError(err => {
                                this.authService.logout();
                                this.router.navigate(['/']);
                                return throwError(err);
                            })
                        );
                    }
                    else if (error) {
                        console.error(error);
                        return throwError(error);
                    }
                }
                return throwError(error);
            })
        );
    }
}