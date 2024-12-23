import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { UserUpdate } from '@app/models/Identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {

  userUpdate = {} as UserUpdate;
  form!: FormGroup;

  get f(): any {
    return this.form.controls; //será usado para substituir o form.get('tema'). no html por f.tema.
  }

  constructor(public fb: FormBuilder, public accountService: AccountService, private router: Router, private toaster: ToastrService, private spinner: NgxSpinnerService) { }

  ngOnInit(): void {

    this.validation();
    this.carregarUsuario();
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

      }, formOptions
    );
  }

  onSubmit(): void{ //vai parar aqui se o form estiver inválido
    this.atualizarUsuario();

  }

  public atualizarUsuario() {
    this.userUpdate = { ...this.form.value }
    this.spinner.show();

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
