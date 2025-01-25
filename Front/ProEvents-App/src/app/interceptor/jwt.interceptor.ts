import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { User } from '@app/models/Identity/User';
import { AccountService } from '@app/services/account.service';
import { catchError, take } from 'rxjs/operators';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  currentUser = {} as User;

  constructor(private accountService: AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {



    this.accountService.currentUser$.pipe(take(1)).subscribe(user =>
      {
        this.currentUser = user

        if(this.currentUser) {
          request = request.clone(
            {
              setHeaders: {
                Authorization: `Bearer ${this.currentUser.token}` //se der problema: tira o this e muda o currentUser para let currentUser: User;
              }
            }
          );
        }
      }); //ver se o usuario esta logado



    return next.handle(request).pipe(
      catchError(error => {
        if(error) {
          localStorage.removeItem('user')
        }
        return throwError(error);
      })
    ); //mandar a requisicao novamente depois de interceptar
  }
}
