import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, retry, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

const RETRY_COUNT = 2;
const RETRY_STATUS_CODES = [502, 503, 504];

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    retry({
      count: RETRY_COUNT,
      delay: (error, retryCount) => {
        if (RETRY_STATUS_CODES.includes(error.status)) {
          return new Promise(resolve => setTimeout(resolve, 1000 * retryCount));
        }
        return throwError(() => error);
      }
    }),
    catchError(error => {
      const isLoginRequest = req.url.endsWith('/api/auth/login');

      if (error.status === 401 && !isLoginRequest) {
        const authService = inject(AuthService);
        const router = inject(Router);
        authService.logout();
        router.navigate(['/login']);
      }
      return throwError(() => error);
    })
  );
};
