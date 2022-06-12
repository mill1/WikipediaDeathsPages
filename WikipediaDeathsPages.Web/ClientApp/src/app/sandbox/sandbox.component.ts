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
  validSite: boolean;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {  
  }

  public incrementCounter() {
    this.currentCount++;
  }

  onFetchOrig(){    
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

  onFetchTest(){    
    this.isBusy = true;    
    this.http.get<boolean>(this.baseUrl + 'reference/check/https:%2F%2Fwww.nu.nl/John Doe').subscribe(result => {         
    this.validSite = result;      
    this.isBusy = false;          
    }, error => {
      this.isBusy = false;
      console.error(error);}
      );
  }

  onFetch(){    
    this.isBusy = true;    
    let url = 'https://www.britannica.com/biography/Utpal-Dutt';

    console.log('encoded: ' + encodeURIComponent(url));

    this.http.get<boolean>(this.baseUrl + 'reference/check/' + encodeURIComponent(url) + '/Utpal Dutt').subscribe(result => {         
    this.validSite = result;      
    this.isBusy = false;          
    }, error => {
      this.isBusy = false;
      console.error(error);}
      );
  }


}
