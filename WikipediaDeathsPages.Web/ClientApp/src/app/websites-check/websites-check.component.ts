import { Component, OnInit, Inject, } from '@angular/core';
import { HttpClient, } from '@angular/common/http';
import { Website } from '../model/Website';

@Component({
  selector: 'app-websites-check',
  templateUrl: './websites-check.component.html',
  styleUrls: ['./websites-check.component.css']
})
export class WebsitesCheckComponent {

  websites: Website[];
  validWebsitesCount: number;
  checkedWebsitesCount: number;  
  checkStatus: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {  

    this.initializeWebsites();    
    this.validWebsitesCount = this.websites.length;    // makes sure that allSitesOk() is true at init (show #elseBlock2)
    this.checkedWebsitesCount = this.websites.length;  // makes sure button is enabled    
  }

  initializeWebsites(){

    this.websites = [ 
      { name: 'Encyclopædia Britannica Online (1)', 
        url: 'https://www.britannica.com/biography/Utpal-Dutt', 
        firstSearchPhrase: 'Utpal Dutt', secondSearchPhrase: 'August 19, 1993', checkResult: false
      }, 
      { name: 'Encyclopædia Britannica Online (2)', 
        url: 'http://www.britannica.com/EBchecked/topic/18816/Viktor-Amazaspovich-Ambartsumian', 
        firstSearchPhrase: 'Viktor Ambartsumian', secondSearchPhrase: 'August 12, 199996', checkResult: false
      },
      { name: 'The Independent (1)', 
        url: 'https://www.independent.co.uk/news/people/obituary-stanley-woods-1488284.html', 
        firstSearchPhrase: 'Obituary: Stanley Woods', secondSearchPhrase: '<amp-state id="digitalData"', checkResult: false
      },
      { name: 'The Independent (2)', 
        url: 'https://www.independent.co.uk/incoming/obituary-harold-shepherdson-5649167.html', 
        firstSearchPhrase: 'Obituary: Harold Shepherdson', secondSearchPhrase: '<amp-state id="digitalData"', checkResult: false
      },
      { name: 'Internet Broadway Database (IBDB)', 
        url: 'https://www.ibdb.com/broadway-cast-staff/88297', 
        firstSearchPhrase: 'Tom Fuccello', secondSearchPhrase: 'Aug 16, 1993', checkResult: false
      },
      /*
      { name: '', 
        url: '', 
        firstSearchPhrase: '', secondSearchPhrase: '', checkResult: false
      },
      */
    ]
  }

  checkWebsites(){

    this.validWebsitesCount = 0;
    this.checkedWebsitesCount = 0;
    this.checkStatus = 'Checking reference websites...';

    for (let website of this.websites) {
      this.checkWebsite(website);
    }

  }

  checkWebsite(website:Website){            
    
    this.http.get<boolean>(this.baseUrl + 'reference/' + encodeURIComponent(website.url) + '/' + 
    website.firstSearchPhrase + '/' + website.secondSearchPhrase).subscribe(result => { 
    
    website.checkResult = result;

    if(website.checkResult)
      this.validWebsitesCount++;

    this.finalizeCheck();
    }, error => {
      this.finalizeCheck();
      console.error(error);
    });        
  }

  finalizeCheck(){
    this.checkedWebsitesCount++;    
    if (this.checkedWebsitesCount == this.websites.length){
      this.checkStatus = 'Check complete';
    }
  }

  allSitesOk(): boolean{
    //console.log('allSitesOk: ' + (this.validWebsitesCount == this.websites.length) + ' (' + this.validWebsitesCount + '-' + this.websites.length + ')')
    return this.validWebsitesCount == this.websites.length;
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

