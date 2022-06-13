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

  checkWebsites(){

    this.validWebsitesCount = 0;
    this.checkedWebsitesCount = 0;
    this.checkStatus = 'Checking reference websites, please hold on...';

    for (let website of this.websites) {
      this.checkWebsite(website);
    }
  }

  checkWebsite(website:Website){            
    
    console.log(encodeURIComponent(website.url));

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

  initializeWebsites(){

    this.websites = [ 
      { name: 'Encyclopædia Britannica Online (1)', 
        url: 'https://www.britannica.com/biography/Utpal-Dutt', 
        firstSearchPhrase: 'Utpal Dutt', secondSearchPhrase: 'August 19, 1993', checkResult: false
      }, 
      { name: 'Encyclopædia Britannica Online (2)', 
        url: 'http://www.britannica.com/EBchecked/topic/18816/Viktor-Amazaspovich-Ambartsumian', 
        firstSearchPhrase: 'Viktor Ambartsumian', secondSearchPhrase: 'August 12, 1996', checkResult: false
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
      { name: 'Spanish Biographical Dictionary (DB~e)', 
        url: 'https://dbe.rah.es/biografias/19291/emilio-botin-sanz-de-sautuola-y-lopez', 
        firstSearchPhrase: 'Emilio Botín-Sanz de Sautuola y López', secondSearchPhrase: '22.IX.1993', checkResult: false
      },
      { name: 'Biografisch Portaal', 
        url: 'http://www.biografischportaal.nl/persoon/26363517', 
        firstSearchPhrase: 'Peter Joseph Engels', secondSearchPhrase: '13 april 1994', checkResult: false
      },
      { name: 'Frauendatenbank FemBio', 
        url: 'https://www.fembio.org/biographie.php/frau/frauendatenbank?fem_id=6165', 
        firstSearchPhrase: 'Cathérine Collard', secondSearchPhrase: '10. Oktober 1993', checkResult: false
      },
      { name: 'Filmportal', 
        url: 'https://www.filmportal.de/person/8ec7d1f8c8774fd8bcac2d74b60413c2', 
        firstSearchPhrase: 'Gerry Sundquist', secondSearchPhrase: '01.08.1993', checkResult: false
      },
      { name: 'Fichier des décès', 
        url: 'http://deces.matchid.io/id/Jwjy9PxtUQEm', 
        firstSearchPhrase: 'matchID', secondSearchPhrase: 'recherche des décès', checkResult: false  // dummy phrases
      },
      { name: 'Library of Congress', 
        url: 'https://id.loc.gov/authorities/names/n79063009', 
        firstSearchPhrase: 'Brooks, John', secondSearchPhrase: '1993-07-27', checkResult: false
      },
      { name: 'Social Networks and Archival Context', 
        url: 'https://snaccooperative.org/ark:/99166/w62z2kg5', 
        firstSearchPhrase: 'Irvin, Eric', secondSearchPhrase: '1993-07-01', checkResult: false
      },
      { name: 'Bibliothèque nationale de France (1)', 
        url: 'http://data.bnf.fr/ark:/12148/cb14122678c', 
        firstSearchPhrase: 'Irving J. Moore ', secondSearchPhrase: '1993', checkResult: false
      },
      { name: 'Bibliothèque nationale de France (2)', 
        url: 'https://data.bnf.fr/ark:/12148/cb13968003b', 
        firstSearchPhrase: 'Wilbert Prysock', secondSearchPhrase: '1993', checkResult: false
      },
      { name: 'Bibliothèque nationale de France (3)', 
        url: 'https://catalogue.bnf.fr/ark:/12148/cb15531676g', 
        firstSearchPhrase: 'Corona, Isabela', secondSearchPhrase: '1993', checkResult: false
      },            
      { name: 'Baseball-Reference.com', 
        url: 'https://www.baseball-reference.com/search/search.fcgi?search=Tsunemi Tsuda',      // '+' -> ' '
        firstSearchPhrase: 'Tsunemi Tsuda', secondSearchPhrase: '1993-07-20', checkResult: false
      },
      { name: 'Pro-Football-Reference.com', 
        url: 'https://www.pro-football-reference.com/search/search.fcgi?search=Abe Shires',     // '+' -> ' '
        firstSearchPhrase: 'Abe Shires', secondSearchPhrase: '1993-07-23', checkResult: false
      },     
      { name: 'Basketball-Reference.com', 
        url: 'https://www.basketball-reference.com/search/search.fcgi?search=Reggie Lewis',     // '+' -> ' '
        firstSearchPhrase: 'Reggie Lewis', secondSearchPhrase: '1993-07-27', checkResult: false
      }, 
      { name: 'Hockey-Reference.com', 
        url: 'https://www.hockey-reference.com/search/search.fcgi?search=Archie Wilcox',        // '+' -> ' '
        firstSearchPhrase: 'Archie Wilcox', secondSearchPhrase: '1993-08-27', checkResult: false
      },
      { name: 'worldfootball.net', 
        url: 'https://www.worldfootball.net/player_summary/tony-barton/', 
        firstSearchPhrase: 'Tony Barton', secondSearchPhrase: '20.08.1993', checkResult: false
      },
      { name: 'procyclingstats.com', 
        url: 'https://www.procyclingstats.com/rider/alfred-haemerlinck', 
        firstSearchPhrase: 'Alfred Haemerlinck', secondSearchPhrase: '1993-07-10', checkResult: false
      },
      { name: 'where2golf.com', 
        url: 'https://www.where2golf.com/whos-who/payne-stewart', 
        firstSearchPhrase: 'William Payne Stewart', secondSearchPhrase: 'Oct 25, 1999', checkResult: false
      },
      { name: 'The New York Times', 
        url: 'https://www.nytimes.com/1998/03/28/us/david-powers-85-aide-to-john-kennedy-dies.html', 
        firstSearchPhrase: 'David Powers', secondSearchPhrase: '1998-03-28', checkResult: false
      }     
    ]
  }
}

