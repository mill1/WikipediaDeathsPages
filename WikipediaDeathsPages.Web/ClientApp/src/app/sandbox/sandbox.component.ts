import { Component, ElementRef, Inject, ViewChild } from '@angular/core';
import { DeathDateResultDto } from '../dto/DeathDateResultDto';
import { HttpClient, HttpParams } from '@angular/common/http';
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

  onFetchTestStringArray(){    
    this.isBusy = true;    
    let urlParam = 'https://www.britannica.com/biography/Utpal-Dutt';

    let params = new HttpParams();
    const actors = ['Elvis', 'Jane', 'Frances'];
    params.append('actors', JSON.stringify(actors));
    
    //this.http.get(url, { params });
    this.http.get<boolean>(this.baseUrl + 'reference/test', { params }).subscribe(result => {         
    this.validSite = result;      
    this.isBusy = false;          
    }, error => {
      this.isBusy = false;
      console.error(error);}
      );
  }

  onFetch(){    
    this.isBusy = true;    
    let urlParam = encodeURIComponent('https://www.britannica.com/biography/Utpal-Dutt');

    this.http.get<boolean>(this.baseUrl + 'reference/' + urlParam + '/' + 'Utpal Dutt' + '/' + 'August 19, 1993').subscribe(result => {         
    this.validSite = result;      
    this.isBusy = false;          
    }, error => {
      this.isBusy = false;
      console.error(error);}
      );
  }


}
