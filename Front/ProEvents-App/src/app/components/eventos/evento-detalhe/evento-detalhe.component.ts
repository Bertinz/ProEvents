import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';

import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';


@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  evento = {} as Evento;
  form!: FormGroup;
  modoSalvar = 'post';

  get f(): any {
    return this.form.controls; //será usado para substituir o form.get('tema'). no html por f.tema.
  }

  get bsConfig(): any {
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD-MM-YYYY HH:mm',
      containerClass: 'theme-default',
      showWeekNumbers: false
      };
  }

  constructor(
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private router: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
  )
    {
    this.localeService.use('pt-br');
    }

    public carregarEvento(): void {
      const eventoIdParam = this.router.snapshot.paramMap.get('id');

      if (eventoIdParam !== null) {
        this.spinner.show();

        this.modoSalvar = 'put';

        this.eventoService.getEventoById(+eventoIdParam).subscribe({
          next: (evento: Evento) => {
            this.evento = {...evento} //pega cada um dos itens do objeto, e com o ... atribui para o this.evento ("é quase que um automapper")
            this.form.patchValue(this.evento)
          },
          error: (error: any) => {
            this.spinner.hide();
            this.toastr.error('Erro ao tentar carregar evento', 'Erro!');
            console.error(error);
          },
          complete: () => this.spinner.hide(),
        }) //o + converte a string para numero
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
        imagemURL: ['', Validators.required ]
      }
    )
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(campoForm: FormControl): any {
    return {'is-invalid': campoForm.errors && campoForm.touched }
  }

  public salvarAlteracao(): void {
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
        next: () => {
          this.toastr.success("Evento salvo com sucesso!", "Sucesso")
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

  }

