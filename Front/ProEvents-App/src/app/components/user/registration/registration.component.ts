import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { User } from '@app/models/Identity/User';
import { AccountService } from '@app/services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

  user = {} as User;
  form!: FormGroup;

  get f(): any {
    return this.form.controls; //será usado para substituir o form.get('tema'). no html por f.tema.
  }

  constructor(private fb: FormBuilder, private accountService: AccountService, private router: Router, private toaster: ToastrService) { }

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
        userName: ['', Validators.required],
        password: ['', [Validators.required, Validators.minLength(4)]],
        confirmPassword: ['', [Validators.required]],

      }, formOptions
    );
  }

  register():void {
    // spread operator = ...
    this.user = { ...this.form.value};
    this.accountService.register(this.user).subscribe(
      () => this.router.navigateByUrl('/dashboard'),
      (error:any) => this.toaster.error(error.error)
    )
  }
}
