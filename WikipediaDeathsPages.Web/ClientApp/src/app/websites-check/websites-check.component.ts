import { Component, OnInit, Inject, } from '@angular/core';
import { HttpClient, } from '@angular/common/http';

@Component({
  selector: 'app-websites-check',
  templateUrl: './websites-check.component.html',
  styleUrls: ['./websites-check.component.css']
})
export class WebsitesCheckComponent {

  urls: string[];
  MAXWEBSITES: number;
  validWebsites: number;
  checkedWebsites: number;
  checkResults: boolean[];
  checkStatus: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {  

    this.initializeWebsites();
    this.MAXWEBSITES = this.urls.length;
    this.validWebsites = this.MAXWEBSITES;    // makes sure that allSitesOk() is true at init (show #elseBlock2)
    this.checkedWebsites = this.MAXWEBSITES;  // makes sure button is enabled  
    this.checkResults = new Array(this.MAXWEBSITES).fill(false);
  }

  initializeWebsites(){

    this.urls=[
      'https://www.britannica.com/biography/Utpal-Dutt',
      'http://www.britannica.com/EBchecked/topic/18816/Viktor-Amazaspovich-Ambartsumian',
      'https://www.independent.co.uk/news/people/obituary-stanley-woods-1488284.html',
      'https://www.independent.co.uk/incoming/obituary-harold-shepherdson-5649167.html', 
      'https://www.ibdb.com/broadway-cast-staff/88297' 
    ];
  }

  checkWebsites(){
    this.validWebsites = 0;
    this.checkedWebsites = 0;
    this.checkStatus = 'Checking reference websites...';
    this.checkWebsite(0, this.urls[0], 'Utpal Dutt', 'August 19, 1993');
    this.checkWebsite(1, this.urls[1], 'Viktor Ambartsumian', 'August 12, 1996');
    this.checkWebsite(2, this.urls[2], 'Obituary: Stanley Woods', '<amp-state id="digitalData"');
    this.checkWebsite(3, this.urls[3], 'Obituary: Harold Shepherdson', '<ap-state id="digitalData"');
    this.checkWebsite(4, this.urls[4], 'Tom Fuccello', 'Aug 16, 1993');
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

