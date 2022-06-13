import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import {DatePipe} from '@angular/common';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { SandboxComponent } from './sandbox/sandbox.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDatepickerConfig, BsDatepickerModule, BsDaterangepickerConfig } from 'ngx-bootstrap/datepicker';
import { DeathDayComponent } from './death-day/death-day.component';
import { NgbdSortableHeader } from './wikidata-details/wikidata-details.component';
import { WikidataDetailsComponent } from './wikidata-details/wikidata-details.component';
import { WebsitesCheckComponent } from './websites-check/websites-check.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    SandboxComponent,
    DeathDayComponent,
    NgbdSortableHeader,
    WikidataDetailsComponent,
    WebsitesCheckComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    BsDatepickerModule.forRoot(),
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'sandbox', component: SandboxComponent },
      { path: 'death-day', component: DeathDayComponent },
    ])
  ],
  providers: [
    DatePipe, 
    BsDatepickerConfig, 
    BsDaterangepickerConfig],
  bootstrap: [AppComponent]
})
export class AppModule { }
