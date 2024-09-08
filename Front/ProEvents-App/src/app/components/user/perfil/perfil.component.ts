import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidatorField } from '@app/helpers/ValidatorField';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {

  form!: FormGroup;

  get f(): any {
    return this.form.controls; //será usado para substituir o form.get('tema'). no html por f.tema.
  }

  constructor(public fb: FormBuilder) { }

  ngOnInit(): void {

    this.validation();
  }

  private validation(): void {//retornam nada, só faz a validação
    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmPassword')
    };

    this.form = this.fb.group(
      {
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        telefone: ['', Validators.required],
        title: ['', Validators.required],
        function: ['', Validators.required],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', [Validators.required]],

      }, formOptions
    );
  }

  onSubmit(): void{ //vai parar aqui se o form estiver inválido
    if(this.form.invalid){
      return;
    }
  }

  public resetForm(event: any): void{
    event?.preventDefault;
    this.form.reset();
  }
}
