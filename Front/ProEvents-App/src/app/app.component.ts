import { Component } from '@angular/core';
import { AccountService } from './services/account.service';
import { User } from './models/Identity/User';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(public accountService: AccountService) {}

  ngOnInit(): void{
    this.setCurrentUser(); //serve para dar refresh na pagina e corrigir o bug do menu reaparecendo
  }

  setCurrentUser(): void {
    let user: User; //erro pode estar aqui

    if (localStorage.getItem('user'))
      user = JSON.parse(localStorage.getItem('user') ?? '{}');
    else
      user = null as any;


    if(user)
      this.accountService.setCurrentUser(user);
  }
}
