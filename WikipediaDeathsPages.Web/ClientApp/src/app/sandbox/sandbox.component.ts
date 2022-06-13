import { Component, Inject, } from '@angular/core';
import { HttpClient, } from '@angular/common/http';

@Component({
  selector: 'app-sandbox-component',
  templateUrl: './sandbox.component.html'
})
export class SandboxComponent {

  MAXWEBSITES = 3;
  validWebsites = this.MAXWEBSITES;    // makes sure that allSitesOk() is true at init (show #elseBlock2)
  checkedWebsites = this.MAXWEBSITES;  // makes sure button is enabled  
  checkResults: boolean[] = new Array(this.MAXWEBSITES).fill(false);
  checkStatus: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {  
  }

  checkWebsites(){
    this.validWebsites = 0;
    this.checkedWebsites = 0;
    this.checkStatus = 'Checking reference websites...';
    this.checkWebsite(0, 'https://www.britannica.com/biography/Utpal-Dutt', 'Utpal Dutt', 'August 19, 1993');    
    this.checkWebsite(2, 'https://www.independent.co.uk/news/people/obituary-stanley-woods-1488284.html', 'Obituary: Stanley Woooods', '<amp-state id="digitalData"');    
    this.checkWebsite(4, 'https://www.ibdb.com/broadway-cast-staff/88297', 'Tom Fuccello', 'Aug 16, 1993');
  }

  checkWebsite(index: number, url:string, firstSearchPhrase: string, secondSearchPhrase: string){            
    
    this.http.get<boolean>(this.baseUrl + 'reference/' + encodeURIComponent(url) + '/' + firstSearchPhrase + '/' + secondSearchPhrase).subscribe(result => {         
    this.checkResults[index] = result;      

    if(this.checkResults[index])
      this.validWebsites++;

    this.finalizeCheck();
    }, error => {
      this.finalizeCheck();
      console.error(error);
    });        
  }

  finalizeCheck(){
    this.checkedWebsites++;    
    if (this.checkedWebsites == this.MAXWEBSITES){
      this.checkStatus = 'Check complete';
    }
  }

  allSitesOk(): boolean{
    //console.log('allSitesOk: ' + (this.validWebsites == this.MAXWEBSITES) + ' (' + this.validWebsites + '-' + this.MAXWEBSITES + ')')
    return this.validWebsites == this.MAXWEBSITES;
  }

  getImageSource(found:boolean):string{
    if(found){
     return "../../assets/ok.png";
    }
    else{
     return "../../assets/nok.png";
    }
  }

}
