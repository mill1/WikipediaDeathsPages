import { Component, Inject, } from '@angular/core';
import { HttpClient, } from '@angular/common/http';

@Component({
  selector: 'app-sandbox-component',
  templateUrl: './sandbox.component.html'
})
export class SandboxComponent {

  MAXWEBSITES =   1;
  validWebsites = 1; // makes sure that allSitesOk() is true at init
  checkedWebsites = 0;
  checkingWebsites = false;
  checkResults: boolean[] = new Array(this.MAXWEBSITES).fill(false);

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {  
  }

  checkWebsites(){
    this.validWebsites = 0;
    this.checkedWebsites = 0;
    this.checkingWebsites = true;
    this.checkWebsite(0, 'https://www.britannica.com/biography/Utpal-Dutt', 'Utpal Dutt', 'August 19, 1993');    
    //this.checkWebsite('https://www.independent.co.uk/news/people/obituary-stanley-woods-1488284.html', 'Obituary: Stanley Woods', '<amp-state id="digitalData"');
    
  }

  checkWebsite(index: number, url:string, firstSearchPhrase: string, secondSearchPhrase: string){            

    this.checkedWebsites++;
    console.log('checking Encyclopædia Britannica...');
    this.http.get<boolean>(this.baseUrl + 'reference/' + encodeURIComponent(url) + '/' + firstSearchPhrase + '/' + secondSearchPhrase).subscribe(result => {         
    this.checkResults[index] = result;      
    
    // TODO
    //this.checkResults[index] = false;

    if(this.checkResults[index])
      this.validWebsites++;

    if(this.checkedWebsites == this.MAXWEBSITES)
      this.checkingWebsites = false;

    }, error => {
      console.error(error);}
      );
  }

  public allSitesOk(){
    console.log('allSitesOk: ' + (this.validWebsites == this.MAXWEBSITES))
    return this.validWebsites == this.MAXWEBSITES;
  }

  //  -----

  getImageSource(found:boolean):string{
    if(found){
     return "../../assets/ok.png";
    }
    else{
     return "../../assets/nok.png";
    }
  }

  /*
  onFetch(){    
    this.notAllSitesOk = true;    
    let urlParam = encodeURIComponent('https://www.britannica.com/biography/Utpal-Dutt');

    this.http.get<boolean>(this.baseUrl + 'reference/' + urlParam + '/' + 'Utpal Dutt' + '/' + 'August 19, 1993').subscribe(result => {         
    this.validSiteBritannica = result;      
    this.notAllSitesOk = false;          
    }, error => {
      this.notAllSitesOk = false;
      console.error(error);}
      );
  }
  */

}
