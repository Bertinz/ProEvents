import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Evento } from '../models/Evento';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from '@environments/environment';
import { PaginatedResult } from '@app/models/Pagination';

@Injectable() //providedIn: 'root 'pode injetar esse serviço em qualquer classe/componente)

export class EventoService {

baseURL = `${environment.apiURL}api/eventos`;


//interceptor vai interceptar qualquer requisicao http na aplicacao, vai clonar ela, adicionar o header dentro da requisicao e colocar uma propriedade nesse cabecalho (bearer token)
constructor(private http: HttpClient) { }

public getEventos(page?: number, itemsPerPage?: number, term?: string): Observable<PaginatedResult<Evento[]>>
{
  const paginatedResult: PaginatedResult<Evento[]> = new PaginatedResult<Evento[]>();

  let params = new HttpParams;

  if(page != null && itemsPerPage != null){
    params = params.append('pageNumber', page.toString());
    params = params.append('pageSize', itemsPerPage.toString());
  }

  if(term != null && term != '')
    params = params.append('term', term);

  return this.http.get<Evento[]>(this.baseURL, {observe: 'response', params } )
          .pipe(take(1), map((response) => {
            paginatedResult.result = response.body as Evento[];
            if(response.headers.has('Pagination')) {
              paginatedResult.pagination = JSON.parse(response.headers.get('Pagination') as string);
            }
            return paginatedResult;
          })); //map manipula o resultado antes de entregar ele pra quem esta se inscrevendo no observable

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
