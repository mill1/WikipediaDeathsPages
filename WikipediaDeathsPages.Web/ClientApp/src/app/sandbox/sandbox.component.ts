import { Component, ElementRef, Inject, ViewChild } from '@angular/core';
import { DeathDateResultDto } from '../dto/DeathDateResultDto';
import { HttpClient } from '@angular/common/http';
import { DeathEntryDto } from '../dto/DeathEntryDto';

@Component({
  selector: 'app-sandbox-component',
  templateUrl: './sandbox.component.html'
})
export class SandboxComponent {

  public currentCount = 0;
  isBusy = false;
  scoreNumberFour: number = -1;
  entries: DeathEntryDto[];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {  
  }

  public incrementCounter() {
    this.currentCount++;
  }

  onFetch(){    
    this.isBusy = true;    

    this.http.get<DeathDateResultDto>(this.baseUrl + 'wikipedia/1984-1-17/48').subscribe(result => {   
      
      this.scoreNumberFour = result.scoreNumberFour;
      this.entries = result.entries;        

      this.isBusy = false;          
    }, error => {
      this.isBusy = false;
      console.error(error);}
      );
  }
}
