import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  public eventos: any = []; //possui elementos vazios, para o .length em evento.component.html funcionar
  public eventosFiltrados: any = []


  widthImg: number = 150;
  marginImg: number = 2;
  showImg: boolean = true;
  private _listFilter: string = '';

  public get listFilter(): string {
    return this._listFilter;

  }

  public set listFilter(value: string) {
    this._listFilter = value;
    this.eventosFiltrados = this.listFilter ? this.EventFilter(this.listFilter) : this.eventos;
  }

  EventFilter(filterBy: string): any {
    filterBy = filterBy.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: {tema: string, local: string; }) => evento.tema.toLocaleLowerCase().indexOf(filterBy) !== -1 ||
                                                   evento.local.toLocaleLowerCase().indexOf(filterBy) !== -1
    );
  }


  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getEventos();
  }

  AlterImage() {
    this.showImg = !this.showImg;
  } //toda vez que clicar em mostrar, mostrará o oposto.

  public getEventos(): void {
    this.http.get('https://localhost:5001/api/eventos').subscribe(
      response => {
        this.eventos = response;
        this.eventosFiltrados = this.eventos;
      },
      error => console.log(error)
    );
  }

}
