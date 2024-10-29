import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { Evento } from '@app/models/Evento';
import { Lote } from '@app/models/Lote';
import { EventoService } from '@app/services/evento.service';
import { LoteService } from '@app/services/lote.service';

import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';


@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  modalRef?: BsModalRef;
  eventoId!: number;
  evento = {} as Evento;
  form!: FormGroup;
  modoSalvar = 'post';
  loteAtual = {id: 0, nome: '', indice: 0};

  get modoEditar(): boolean {
    return this.modoSalvar === 'put';
  }

  get lotes(): FormArray {
    return this.form.get('lotes') as FormArray;
  }

  get f(): any {
    return this.form.controls; //será usado para substituir o form.get('tema'). no html por f.tema.
  }

  get bsConfig(): any {
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY HH:mm',
      containerClass: 'theme-default',
      showWeekNumbers: false
      };
  }

  constructor(
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private activatedRouter: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private modalService: BsModalService,
    private router: Router,
    private loteService: LoteService,
  )
    {
    this.localeService.use('pt-br');
    }

    public carregarEvento(): void {
      this.eventoId = +this.activatedRouter.snapshot.paramMap.get('id')!;

      if (this.eventoId !== null && this.eventoId !== 0) {
        this.spinner.show();

        this.modoSalvar = 'put';

        this.eventoService.getEventoById(this.eventoId).subscribe({
          next: (evento: Evento) => {
            this.evento = {...evento} //pega cada um dos itens do objeto, e com o ... atribui para o this.evento ("é quase que um automapper")
            this.form.patchValue(this.evento)
            this.evento.lotes.forEach(lote =>  {
              this.lotes.push(this.criarLote(lote)) //lotes serão pegos via callback, dando um push e criando um formulário passando lotes
            });
          },
          error: (error: any) => {
            this.spinner.hide();
            this.toastr.error('Erro ao tentar carregar evento', 'Erro!');
            console.error(error);
          }
        }).add(() => this.spinner.hide()) //o + converte a string para numero
      }
    }


  ngOnInit(): void {
    this.carregarEvento();
    this.validation();
  }

  public validation(): void{
    this.form = this.fb.group(
      {
        tema: ['',[Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
        local: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(30)]], //validators.required = preenchimento obrigatório
        dataEvento: ['', Validators.required],
        qtdPessoas: ['',[Validators.required, Validators.max(120000)]],
        telefone: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        imagemURL: ['', Validators.required ],
        lotes: this.fb.array([

        ]) //a cada item que for adicionando, estará dentro de um array
      }
    )
  }

  adicionarLote(): void {
    this.lotes.push(this.criarLote({id: 0} as Lote));
  }

  criarLote(lote: Lote): FormGroup {
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim]

    });
  }

  public mudarValorData(value: Date, indice: number, campo: string): void {
    this.lotes.value[indice][campo] = value;
  }

  public retornaTituloLote(nome: string) : string {
    return nome === null || nome === '' ? 'Nome do lote' : nome;
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(campoForm: FormControl | AbstractControl): any {
    return {'is-invalid': campoForm.errors && campoForm.touched }
  }

  public salvarEvento(): void {
    this.spinner.show();
    if(this.form.valid) {

      let service = {} as Observable<Evento>;

      if (this.modoSalvar === 'post') {
        this.evento = {...this.form.value}
        service = this.eventoService.post(this.evento);
      } else {
        this.evento = {id: this.evento.id, ...this.form.value} // quero que copie todos os dados, mas adicione o id, vindo do this.evento.id
        service = this.eventoService.put(this.evento);
      };

      service.subscribe({
        next: (eventoRetorno: Evento) => {
          this.toastr.success("Evento salvo com sucesso!", "Sucesso")
          this.router.navigate([`eventos/detalhe/${eventoRetorno.id}`])
        },
        error: (error: any) => {
          this.spinner.hide();
          this.toastr.error('Erro ao salvar evento', 'Erro');
          console.error(error);
        },
        complete: () => this.spinner.hide(),
      });
    }

  }
  public salvarLote(): void {
    if(this.form.controls.lotes.valid){
      this.spinner.show();
      this.loteService.put(this.eventoId, this.form.value.lotes)
      .subscribe(
        () => {
          this.toastr.success('Lotes salvos com sucesso!', 'Sucesso!');
        },
        (error: any) => {
          this.toastr.error('Erro ao tentar salvar lotes.', 'Erro');
          console.error(error);
        }
      ).add(() => this.spinner.hide());
    }
  }

  public removerLote(template: TemplateRef<any>, indice: number): void{
    this.loteAtual.id = this.lotes.get(indice + '.id')?.value;
    this.loteAtual.nome = this.lotes.get(indice + '.nome')?.value;
    this.loteAtual.indice = indice;


    this.modalRef = this.modalService.show(template, {class: 'modal-sm' });


  }

  confirmDeleteLote(): void {
    this.modalRef?.hide();
    this.spinner.show();
    this.loteService.deleteLote(this.eventoId, this.loteAtual.id).subscribe(
      () => {
        this.toastr.success('Lote deletado com sucesso', 'Sucesso');
        this.lotes.removeAt(this.loteAtual.indice);
      },
      (error: any) => {
        this.toastr.error(`Erro ao tentar deletar o lote ${this.loteAtual.id}`, 'Erro');
        console.error(error);
      }
    ).add(() => this.spinner.hide());;
  }

  declineDeleteLote(): void {
    this.modalRef?.hide();
  }



}



