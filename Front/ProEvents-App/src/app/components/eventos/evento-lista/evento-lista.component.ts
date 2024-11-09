import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {

  modalRef?: BsModalRef;
  public eventos: Evento[] = []; //possui elementos vazios, para o .length em evento.component.html funcionar
  public eventosFiltrados: Evento[] = []
  public eventoId = 0;


  public widthImg = 150;
  public marginImg = 2;
  public showImg = true;
  private listedFilter = '';

  public get listFilter(): string {
    return this.listedFilter;

  }

  public set listFilter(value: string) {
    this.listedFilter = value;
    this.eventosFiltrados = this.listFilter ? this.EventFilter(this.listFilter) : this.eventos;
  }

  public EventFilter(filterBy: string): Evento[] {
    filterBy = filterBy.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: {tema: string, local: string; }) => evento.tema.toLocaleLowerCase().indexOf(filterBy) !== -1 ||
                                                   evento.local.toLocaleLowerCase().indexOf(filterBy) !== -1
    );
  }


  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) { }

  public ngOnInit(): void {
    this.spinner.show();
    this.getEventos();
  }

  public AlterImage(): void {
    this.showImg = !this.showImg;
  } //toda vez que clicar em mostrar, mostrará o oposto.

  public mostrarImagem(imagemURL: string): string {
    return imagemURL !== '' ? `${environment.apiURL}resources/images/${imagemURL}` : 'assets/img/semImagem.jpg';
  }

  public getEventos(): void {


    //observable -> next, error, complete
    this.eventoService.getEventos().subscribe
    ({
        next: (eventos: Evento[]) => { //_eventos era response, antes de tiparmos os components em services
          this.eventos = eventos;
          this.eventosFiltrados = this.eventos;
        },
        error: (error:any) => {
          this.spinner.hide();
          this.toastr.error('Failed to load the events', 'Error!');
        },
        complete: () => this.spinner.hide(),
      });
  }

  openModal(event: any, template: TemplateRef<any>, eventoId: number) : void {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  confirm(): void {
    this.modalRef?.hide();
    this.spinner.show();

    this.eventoService.deleteEvento(this.eventoId).subscribe({
      next: (result: string) => {
          console.log(result);
          this.toastr.success('Evento deletado com sucesso', 'Deletado!');
          this.getEventos();
      },
      error: (error:any) => {
        this.toastr.error(`Erro ao tentar deletar o evento ${this.eventoId}`, 'Erro');

        console.error(error);
      },
      complete: () => {},

    }).add(() => this.spinner.hide());

  }

  decline(): void {
    this.modalRef?.hide();
    this.toastr.success('Evento não deletado', 'Cancelado');
  }

  detalheEvento(id: number): void {
    this.router.navigate([`eventos/detalhe/${id}`]);
  }

}
