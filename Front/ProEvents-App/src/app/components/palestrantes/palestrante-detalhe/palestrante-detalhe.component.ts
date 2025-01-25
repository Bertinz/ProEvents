import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Palestrante } from '@app/models/Palestrante';
import { PalestranteService } from '@app/services/palestrante.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { debounceTime, map, tap } from 'rxjs/operators';

@Component({
  selector: 'app-palestrante-detalhe',
  templateUrl: './palestrante-detalhe.component.html',
  styleUrls: ['./palestrante-detalhe.component.scss']
})
export class PalestranteDetalheComponent implements OnInit {

  form!: FormGroup;
  situacaoDoForm = '';
  corDaDescricao = '';


  constructor(private fb: FormBuilder, public palestranteService: PalestranteService, private toaster: ToastrService, private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.validation();
    this.verificaForm();
    this.carregarPalestrante();
  }

  private validation(): void{
    this.form = this.fb.group({
      miniCurriculo: ['']
    })
  }

  private carregarPalestrante(): void {
    this.spinner.show();

    this.palestranteService.getPalestrante().subscribe(
      (palestrante: Palestrante) => {
        this.form.patchValue(palestrante)
    },
      (error: any) => {
        this.toaster.error('Erro ao carregar o Palestrante','Erro')
      }
    )
  }

  public get f(): any{
    return this.form.controls;
  }

  private verificaForm(): void{
    this.form.valueChanges.pipe(
      map(() => {
        this.situacaoDoForm = 'Minicurriculo esta sendo atualizado!'
        this.corDaDescricao = 'text-warning'

      }),
      debounceTime(1500),
      tap(() => this.spinner.show())
    ).subscribe( //se inscrevendo no form para fazer o load
      () => {
        this.palestranteService.put({...this.form.value}).subscribe(
          () => {//se inscrevendo para atualizar
            this.situacaoDoForm = 'MiniCurriculo foi Atualizado!'
            this.corDaDescricao = 'text-success'

            setTimeout(() => {
              this.situacaoDoForm = 'MiniCurriculo foi Carregado!'
              this.corDaDescricao = 'text-muted'
            })
          },
          () => {
            this.toaster.error('Erro ao tentar atualizar palestrante', 'Erro')
          }
        ).add(() => this.spinner.hide())
      })
  }

}
