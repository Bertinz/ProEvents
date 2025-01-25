import { Component, OnInit } from '@angular/core';
import { UserUpdate } from '@app/models/Identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { environment } from '@environments/environment';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {

  usuario = {} as UserUpdate;
  imagemURL = '';
  file = {} as File;

  public get ehPalestrante(): boolean {
    return this.usuario.funcao === 'Palestrante';
  }



constructor(
  private spinner: NgxSpinnerService,
  private toaster: ToastrService,
  private accountService: AccountService
) {

}

  ngOnInit(): void {

  }

  public setFormValue(usuario: UserUpdate): void {
    this.usuario = usuario;
    if(this.usuario.imagemURL){
      this.imagemURL = environment.apiURL + `resources/perfil/${this.usuario.imagemURL}`;
    }
    else{
      this.imagemURL = './assets/img/perfil.png';
    }
  }

  onFileChange(ev: any): void{
    const reader = new FileReader();

    reader.onload = (event: any) => this.imagemURL = event.target.result;

    this.file = ev.target.files;
    reader.readAsDataURL(this.file[0 as keyof undefined]);

    this.uploadImagem();

  }

  private uploadImagem(): void{
    this.spinner.show();
    this.accountService.postUpload(this.file).subscribe(
      () => this.toaster.success('Imagem atualizada com Sucesso', 'Sucesso!'),
      (error: any) => {
        this.toaster.error('Erro ao fazer upload de imagem', 'Erro!');
        console.error(error);
      }
    ).add(() => this.spinner.hide())
  }



}
