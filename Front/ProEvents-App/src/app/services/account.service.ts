import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '@app/models/Identity/User';
import { UserUpdate } from '@app/models/Identity/UserUpdate';
import { environment } from '@environments/environment';
import { Observable, ReplaySubject } from 'rxjs';
import { map, take } from 'rxjs/operators';

@Injectable({
  providedIn: null
})
export class AccountService {
  private currentUserSource = new ReplaySubject<User>(1); //essa variavel recebe diversas atualizacoes, sendo acessado somente pelo service
  public currentUser$ = this.currentUserSource.asObservable();
  //observable -> nao precisa ficar chamando o localstorage toda hora

  baseUrl = environment.apiURL + 'api/account/';

  constructor(private http: HttpClient) { }

  public login(model: any): Observable<void>{
    return this.http.post<User>(this.baseUrl + 'login', model).pipe(
                                take(1),
                                map((response: User) => {
                                  const user = response;
                                  if (user) {

                                    this.setCurrentUser(user)
                                  }
                                })
  );
  }

  public register(model: any): Observable<void>{
    return this.http.post<User>(this.baseUrl + 'register', model).pipe(
                                take(1),
                                map((response: User) => {
                                  const user = response;
                                  if (user) {

                                    this.setCurrentUser(user)
                                  }
                                })
  );
  }

  getUser(): Observable<UserUpdate> {
    return this.http.get<UserUpdate>(this.baseUrl + 'getUser').pipe(take(1));
  }

  updateUser(model: UserUpdate): Observable<void>{ //receb o que atualizou como parametro, manda o que atualizou para o Userupdate, espera que o UserUpdate seja chamado apenas 1 vez, depois pode desinscrever do http.put e depois um map passando a instrucao
    return this.http.put<UserUpdate>(this.baseUrl + 'updateUser', model).pipe(take(1),
    map((user: UserUpdate) => {
      this.setCurrentUser(user);
    }
  ));
}

logout(): void {
  localStorage.removeItem('user');
  this.currentUserSource.next(null as any); //todo mundo inscrito no observable continua inscrito, mas todos eles naquele momento receberao null, assim voltando para a tela de login
  this.currentUserSource.complete();
}

//erro pode estar aqui
  public setCurrentUser(user: User): void { //ao logar, todo mundo se mantem inscrito e recebe user novamente
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }
}
