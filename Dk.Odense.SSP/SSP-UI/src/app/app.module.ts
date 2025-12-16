import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
//MDB

//modules
import { HomeComponent } from './home/home.component';
import { LoginModule } from './modules/login/login.module';
import { AdministrationModule } from './modules/administration/administration.module';
import { AgendaModule } from './modules/agenda/agenda.module';
import { GroupingsModule } from './modules/groupings/groupings.module';
import { PersonModule } from './modules/person/person.module';
import { CsvUploadModule } from './modules/upload/upload.module';
import { VerifyModule } from './modules/verify/verify.module';
import { SharedModule } from '@shared/shared.module';

@NgModule({
  declarations: [AppComponent, HomeComponent],
  imports: [
    LoginModule,
    AdministrationModule,
    AgendaModule,
    GroupingsModule,
    PersonModule,
    VerifyModule,
    CsvUploadModule,
    SharedModule,
    BrowserModule,
    AppRoutingModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
