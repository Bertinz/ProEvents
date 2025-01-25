import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { UserUpdate } from '@app/models/Identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { PalestranteService } from '@app/services/palestrante.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-perfil-detalhe',
  templateUrl: './perfil-detalhe.component.html',
  styleUrls: ['./perfil-detalhe.component.scss']
})
export class PerfilDetalheComponent implements OnInit {
  @Output() changeFormValue = new EventEmitter(); //passar um valor da classe filha(perfilDetalhe) para classe pai(perfil)

  userUpdate = {} as UserUpdate;
  form!: FormGroup;


  get f(): any {
    return this.form.controls; //será usado para substituir o form.get('tema'). no html por f.tema.
  }

  constructor(public fb: FormBuilder, public accountService: AccountService, public palestranteService: PalestranteService, private router: Router, private toaster: ToastrService, private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.validation();
    this.carregarUsuario();
    this.verificaForm();
  }

  private verificaForm(): void { //qualquer informacao que for alterada em qualquer componente, deve ser realizada uma execucao, um subscribe
    this.form.valueChanges.subscribe(
      () => this.changeFormValue.emit({...this.form.value}) //spread operator para pegar cada valor do form para entao emitir para o changeFormValue que ira emitir para o app-perfil-component
    )
  }

  private carregarUsuario(): void {
    this.spinner.show();
    this.accountService
      .getUser()
      .subscribe(
        (userRetorno: UserUpdate) => {
          console.log(userRetorno);
          this.userUpdate = userRetorno;
          this.form.patchValue(this.userUpdate);
          this.toaster.success('Usuário Carregado', 'Sucesso');
        },
        (error) => {
          console.error(error);
          this.toaster.error('Usuário não Carregado', 'Erro');
          this.router.navigate(['/dashboard']);
        }
      )
      .add(() => this.spinner.hide());
    }

  private validation(): void {//retornam nada, só faz a validação
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmPassword')
    };

    this.form = this.fb.group(
      {
        userName: [''],
        titulo: ['', Validators.required],
        primeiroNome: ['NaoInformado', Validators.required],
        ultimoNome: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        phoneNumber: ['', Validators.required],
        descricao: ['', Validators.required],
        funcao: ['NaoInformado', Validators.required],
        password: ['', [, Validators.minLength(6), Validators.nullValidator]],
        confirmPassword: ['', [Validators.nullValidator]],
        imagemURL: [''],

      }, formOptions
    );
  }

  onSubmit(): void{ //vai parar aqui se o form estiver inválido
    this.atualizarUsuario();

  }

  public atualizarUsuario() {
    this.userUpdate = { ...this.form.value }
    this.spinner.show();

    if(this.f.funcao.value == 'Palestrante'){
      //chamo a funcao de criar o registro na tabela de Palestrante
      this.palestranteService.post().subscribe(
        () => this.toaster.success('Palestrante ativada!', 'Sucesso!'),
        (error) => {
          this.toaster.error('Palestrante nao foi ativado!', 'Error');
          console.error(error);
        }
      )
    }

    this.accountService.updateUser(this.userUpdate).subscribe(
      () => this.toaster.success('Usuario atualizado.', 'Sucesso'),
      (error) => {
        this.toaster.error('Usuario nao atualizado.', 'Erro');
        console.error(error);
      },
    ).add(() => this.spinner.hide())
  }

  public resetForm(event: any): void{
    event?.preventDefault;
    this.form.reset();
  }

}
