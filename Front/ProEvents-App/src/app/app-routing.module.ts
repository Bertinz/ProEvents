import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { EventosComponent } from './components/eventos/eventos.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { PalestrantesComponent } from './components/palestrantes/palestrantes.component';
import { ContatosComponent } from './components/contatos/contatos.component';
import { PerfilComponent } from './components/perfil/perfil.component';

const routes: Routes = [
  { path: 'eventos', component: EventosComponent},
  { path: 'dashboard', component: DashboardComponent},
  { path: 'palestrantes', component: PalestrantesComponent},
  { path: 'contatos', component: ContatosComponent},
  { path: 'contatos', component: ContatosComponent},
  { path: 'perfil', component: PerfilComponent},
  { path: '', redirectTo: 'dashboard', pathMatch: 'full'}, //se colocar nada, envia para dashboard
  { path: '**', redirectTo: 'dashboard', pathMatch: 'full'} //se colocar qualquer coisa fora das opções, vai para dashboard

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
