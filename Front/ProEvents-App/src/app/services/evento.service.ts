import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Evento } from '../models/Evento';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from '@environments/environment';

@Injectable() //providedIn: 'root 'pode injetar esse serviço em qualquer classe/componente)

export class EventoService {

baseURL = `${environment.apiURL}api/eventos`;


//interceptor vai interceptar qualquer requisicao http na aplicacao, vai clonar ela, adicionar o header dentro da requisicao e colocar uma propriedade nesse cabecalho (bearer token)
constructor(private http: HttpClient) { }

public getEventos(): Observable<Evento[]>
{
  return this.http.get<Evento[]>(this.baseURL )
          .pipe(take(1));

}

public getEventosByTema(tema: string): Observable<Evento[]>
{
  return this.http.get<Evento[]>(`${this.baseURL}/${tema}/tema`)
          .pipe(take(1));


}

public getEventoById(id: number): Observable<Evento>
{
  return this.http.get<Evento>(`${this.baseURL}/${id}`)
  .pipe(take(1));

}

public post(evento: Evento): Observable<Evento>
{
  return this.http.post<Evento>(this.baseURL, evento) //como não está editando nada, passa apenas URL e evento de parâmetro
  .pipe(take(1));
}

public put(evento: Evento): Observable<Evento>
{
  return this.http.put<Evento>(`${this.baseURL}/${evento.id}`, evento)
  .pipe(take(1));

}

public deleteEvento(id: number): Observable<any>
{
  return this.http.delete(`${this.baseURL}/${id}`)
  .pipe(take(1));

}

postUpload(eventoId: number, file: any): Observable<Evento>{
  const fileToUpload = file[0] as File;
  const formData = new FormData();
  formData.append('file', fileToUpload)

  return this.http.post<Evento>(`${this.baseURL}/upload-image/${eventoId}`, formData) //como não está editando nada, passa apenas URL e evento de parâmetro
  .pipe(take(1));
}


}
