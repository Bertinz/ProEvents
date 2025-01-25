import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PaginatedResult } from '@app/models/Pagination';
import { Palestrante } from '@app/models/Palestrante';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PalestranteService {

baseURL = `${environment.apiURL}api/palestrantes`;


//interceptor vai interceptar qualquer requisicao http na aplicacao, vai clonar ela, adicionar o header dentro da requisicao e colocar uma propriedade nesse cabecalho (bearer token)
constructor(private http: HttpClient) { }

public getPalestrantes(page?: number, itemsPerPage?: number, term?: string): Observable<PaginatedResult<Palestrante[]>>
{
  const paginatedResult: PaginatedResult<Palestrante[]> = new PaginatedResult<Palestrante[]>();

  let params = new HttpParams;

  if(page != null && itemsPerPage != null){
    params = params.append('pageNumber', page.toString());
    params = params.append('pageSize', itemsPerPage.toString());
  }

  if(term != null && term != '')
    params = params.append('term', term);

  return this.http.get<Palestrante[]>(this.baseURL + '/all', {observe: 'response', params } )
          .pipe(take(1), map((response) => {
            paginatedResult.result = response.body as Palestrante[];
            if(response.headers.has('Pagination')) {
              paginatedResult.pagination = JSON.parse(response.headers.get('Pagination') as string);
            }
            return paginatedResult;
          })); //map manipula o resultado antes de entregar ele pra quem esta se inscrevendo no observable

}

public getPalestrantesByTema(tema: string): Observable<Palestrante[]>
{
  return this.http.get<Palestrante[]>(`${this.baseURL}/${tema}/tema`)
          .pipe(take(1));


}

public getPalestrante(): Observable<Palestrante>
{
  return this.http.get<Palestrante>(`${this.baseURL}`) //pega palestrante apenas pelo token (da apenas a base url)
  .pipe(take(1));

}

public post(): Observable<Palestrante>
{
  return this.http.post<Palestrante>(this.baseURL, {} as Palestrante) //como não está editando nada, passa apenas URL e Palestrante de parâmetro
  .pipe(take(1));
}

public put(palestrante: Palestrante): Observable<Palestrante>
{
  return this.http.put<Palestrante>(`${this.baseURL}`, palestrante)
  .pipe(take(1));

}


}
