import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-titulo',
  templateUrl: './titulo.component.html',
  styleUrls: ['./titulo.component.scss']
})
export class TituloComponent implements OnInit {


  @Input()
  titulo!: string;
  @Input()
  subtitulo!: string;
  @Input()
  iconClass!: string;
  @Input()
  botaoListar!: boolean;

  constructor(private router: Router) { }

  ngOnInit() : void {

  }

    list(): void {
      this.router.navigate([`/${this.titulo.toLocaleLowerCase()}/lista`])
    }


}
