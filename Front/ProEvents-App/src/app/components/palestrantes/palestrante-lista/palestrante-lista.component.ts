import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { Palestrante } from '@app/models/Palestrante';
import { PalestranteService } from '@app/services/palestrante.service';
import { environment } from '@environments/environment';
import { BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-palestrante-lista',
  templateUrl: './palestrante-lista.component.html',
  styleUrls: ['./palestrante-lista.component.scss']
})
export class PalestranteListaComponent implements OnInit {

  public palestrantes: Palestrante[] = [];
  public eventoId = 0;
  public pagination = {} as Pagination;

  constructor(
    private palestranteService: PalestranteService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) { }

  ngOnInit() : void {
    this.pagination = {
      currentPage: 1,
      itemsPerPage: 3,
      totalItems: 1,
    } as Pagination;

    this.carregarPalestrantes();
  }

  termoBuscaChanged: Subject<string> = new Subject<string>();

    public PalestranteFilter(palestrante: any): void {
      if (this.termoBuscaChanged.observers.length == 0) {

        this.termoBuscaChanged.pipe(debounceTime(1000)).subscribe(
          filtrarPor => {
            this.spinner.show();
            this.palestranteService.getPalestrantes(this.pagination.currentPage, this.pagination.itemsPerPage, filtrarPor)
            .subscribe(
              {
                next: (paginatedResponse: PaginatedResult<Palestrante[]>) => {
                  this.palestrantes = paginatedResponse.result;
                  this.pagination = paginatedResponse.pagination;
                },
                error: (error:any) => {
                  this.spinner.hide();
                  this.toastr.error('Failed to load the Palestrantes', 'Error!');
                },
                complete: () => this.spinner.hide(),
              }
            )
          }
        )
      }
      this.termoBuscaChanged.next(palestrante.value); //altera o termo para que tenha pelo menos 1 item
    }

    getImagemURL(imagemName: string): string {
      if(imagemName){
        return imagemName = environment.apiURL + `resources/perfil/${imagemName}`;
      }
      else{
        return imagemName = './assets/img/perfil.png';
      }
    }

  public carregarPalestrantes(): void {
    this.spinner.show();

    //observable -> next, error, complete
    this.palestranteService.getPalestrantes(this.pagination.currentPage, this.pagination.itemsPerPage).subscribe
    ({
      next: (paginatedResponse: PaginatedResult<Palestrante[]>) => { //_palestrantes era response, antes de tiparmos os components em services
        this.palestrantes = paginatedResponse.result;
        this.pagination = paginatedResponse.pagination;
      },
      error: (error:any) => {
        this.spinner.hide();
        this.toastr.error('Failed to load the events', 'Error!');
      },
      complete: () => this.spinner.hide(),
    });
  }

}
